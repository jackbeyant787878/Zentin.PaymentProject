# ADR-0006 · MQ 可靠投递采用 CAP 本地消息表（outbox）

- 状态：已采纳
- 日期：2026-11-24

## 背景

供体的支付成功链路存在经典双写：先落库再发 MQ，两步不在同一事务，失败仅记日志。
证据：`agpayplus/src/AGooday.AgPay.Components.Third/Services/PayOrderProcessService.cs:66-71`
（`UpdateDivisionStateAsync` 成功后 `SendAsync(PayOrderDivisionMQ, 60)`，外层 catch 吞异常）。
MQ 发送失败即丢消息，靠定时任务兜底。

Zentin 侧现状：RabbitMQ 只有连接注册与健康检查，无真实发布/消费；领域事件走进程内
MediatR 且在 SaveChanges 之后发布，进程崩溃会丢事件。

需要的是"落库 → 发消息"的原子性与最终一致性，不是跨库分布式事务：

- 模块间同库操作：本地 EF 事务即可（铁律 7）。
- 调渠道 HTTP：外部系统不受任何事务组件覆盖，靠幂等 + 对账。

## 决策

1. 引入 **DotNetCore.CAP 10.0.2** 四件套（Core / SqlServer / RabbitMQ / Dashboard），
   仅在 Infrastructure 层注册（`AddZentinCap`），Domain/Application 不感知 CAP。
2. **存储与业务同库**（`ConnectionStrings:DefaultConnection`），发布消息时与业务写入
   共用同一 DbContext 事务，实现 outbox 原子性。
3. `Cap.Published` / `Cap.Received` 表由 CAP 启动时自建自管，**不纳入 EF 迁移**。
4. 每服务独立消费组（`Cap:GroupName`：zentin-payment / manager / agent / merchant），
   共享交换机 `zentin.cap`；失败重试 5 次（`Cap:FailedRetryCount`）。
5. **至少一次投递 ⇒ 消费端必须幂等**，这是后续每个订阅者的 DoD 项。
6. 延迟消息（对应供体分账延迟 60s）用 CAP 延迟发布能力，不依赖 RabbitMQ 延迟插件。
7. 业务发布/订阅不在本轮落地；第一个真实消息随 PayOrder 切片（支付成功 → 商户通知）
   接入，并配集成测试。

## 后果

好处：

- 消除双写丢消息；重试、失败可视化（Dashboard `/cap`）开箱即用。
- 与既有栈零新增基础设施：SqlServer + RabbitMQ 均已在位。

代价与约束：

- CAP 自建表不受 EF 迁移守卫覆盖，schema 升级随 CAP 包版本走，升级时需 review。
- CAP 后台引导服务启动即连库/连 MQ；本地未起 RabbitMQ 时会持续重试并刷错误日志，
  不阻塞宿主启动。
- 消费端幂等是硬要求，漏做会导致重复记账/重复通知。

## 替代方案

- **手写 outbox 表 + 后台派发器**：无新依赖，但要自造重试、监控、并发控制，否决。
- **直接发 MQ + 定时对账兜底（供体模式）**：实现最简但丢消息窗口真实存在，
  资金链路不可接受，否决。
- **分布式事务（2PC/Saga 框架）**：当前单库模块化单体无跨库写入，属过度设计，否决。

## 补遗：延迟消息（S2 商户通知，2026-11-24）

- 延迟重试采用 CAP 10 原生 `PublishDelayAsync(TimeSpan, name, content, headers, ct)`：
  消息以 `StatusName='Scheduled'` 写入 Cap.Published，到期时间存 `ExpiresAt`，
  由 CAP 后台调度器到期投递；**不依赖 RabbitMQ delayed-message 插件**（已验证包内无 x-delayed 机制）。
- 坑：`headers` 传 `null` 会在 `CapPublisher.PublishInternalAsync` 触发 NRE，
  必须传空字典 `new Dictionary<string, string>()`。即时发布的重载内部自建字典故无此问题。
- 领域事件映射表扩展为 `(Topic, Payload, TimeSpan? Delay)` 三元组；
  `MchNotifyAttemptedDomainEvent` 携带 `DelaySeconds = NotifyCount * 30`（供体重试节奏），
  重试消息与记录状态更新经 UoW 同事务落 outbox，消除供体双写窗口。
# Legacy Parity 0002 · MchNotifyRecord 商户通知链

- 供体：`D:\Projects\agpayplus` @ `ba61fea9`（只读）
- 目标：Zentin.Payment S2 切片（CAP 消费端 + 延迟重试）
- 日期：2026-11-24
- 供体主源：`AGooday.AgPay.Components.Third/Services/PayMchNotifyService.cs`、`AGooday.AgPay.Base.Api/MQ/PayOrderMchNotifyMQReceiver.cs`、`AGooday.AgPay.Common/Utils/AgPayUtil.cs`

## 1. 语义对齐项（与供体一致）

| # | 规则 | 供体证据 | Zentin 落点 |
| --- | --- | --- | --- |
| 1 | NotifyUrl 为空不通知 | PayMchNotifyService.cs:50-53 | PayOrderPaidNotifyConsumer |
| 2 | 按订单去重，已存在不再发送 | PayMchNotifyService.cs:56-62 | GetByOrderAsync + 唯一索引 Uni_OrderId_OrderType |
| 3 | 签名密钥取商户应用 AppSecret | PayMchNotifyService.cs:65 | IMchAppRepository.GetByMchNoAndAppIdAsync |
| 4 | 通知上限 6 次、初始状态通知中 | PayMchNotifyService.cs:86-87 | MchNotifyRecord.Create 默认 limit=6 |
| 5 | MQ 载荷仅 NotifyId | PayMchNotifyService.cs:103-104 | MchNotifyDispatchIntegrationEvent(NotifyId) |
| 6 | 消费守卫：记录不存在/非通知中/次数用尽 → 跳过 | PayOrderMchNotifyMQReceiver.cs:43-52 | MchNotifyDispatchConsumer 守卫 |
| 7 | 首次尝试无论成败回写 PayOrder.NotifyState | PayOrderMchNotifyMQReceiver.cs:95-98 | NotifyCount==0 && OrderType==Pay 分支 |
| 8 | 响应 "SUCCESS"（忽略大小写）终止重试 | PayOrderMchNotifyMQReceiver.cs:101-105 | MarkSuccess 分支 |
| 9 | 次数用尽置失败；否则保持通知中 | PayOrderMchNotifyMQReceiver.cs:108-115 | RecordAttempt 内置 |
| 10 | 重试延迟 = 当前次数 × 30 秒（0/30/60/90/120/150） | PayOrderMchNotifyMQReceiver.cs:117-119 | MchNotifyAttemptedDomainEvent.DelaySeconds |
| 11 | 报文 = 订单 RS 字段 + reqTime(Unix 秒) + sign | PayMchNotifyService.cs:357-387、QueryPayOrderRS.cs:100-117 | MchNotifyPayloadBuilder |
| 12 | 签名：跳过 null/空串/sign，排序拼接后追加 `key={secret}`，MD5 大写 | AgPayUtil.cs:88-94,139-154,161-166 | GetSign 原样移植（含两个怪癖：`key=` 前无 `&`；默认文化敏感排序） |
| 13 | 传输异常转为响应文本进入重试判定 | PayOrderMchNotifyMQReceiver.cs:88-92 | MchNotifySender catch |
| 14 | 默认 POST_JSON 报文 | PayMchNotifyService.cs:334-337 | ReqMediaType=application/json |

## 2. 有意差异（增强，非疏忽）

| # | 差异 | 理由 |
| --- | --- | --- |
| 1 | 重试调度经领域事件 + CAP outbox 同事务落库，替代供体"先更新 DB 再发 MQ"双写 | 消除供体 Receiver:115-119 的崩溃窗口（更新成功但 MQ 未发 → 永久卡死）；ADR-0006 |
| 2 | 延迟投递用 CAP `PublishDelayAsync`（Cap.Published.ExpiresAt + 后台调度器），不依赖 RabbitMQ 延迟插件 | CAP 10 原生能力，已验证 dll 无 x-delayed 依赖 |
| 3 | HTTP 超时 15 秒 | 供体 `TimeSpan.FromSeconds(20000)` 为笔误（20000 秒），Receiver:65 |
| 4 | `MarkSuccess` 重复调用幂等返回 | CAP 至少一次投递；与 PayOrder 聚合级幂等决策一致 |
| 5 | 并发重复创建靠唯一索引 + CAP 重试自愈，而非供体 catch 吞异常 | 供体 Service:93-100 吞掉所有异常会丢真实故障；Zentin 重跑命中去重检查，不丢消息 |
| 6 | 时间统一 UTC 存储，epoch 转换显式 SpecifyKind(Utc) | 供体 DateTime.Now 本地时间；EF 取回 Kind=Unspecified 须显式标记 |
| 7 | 出站 HTTP 经统一韧性管线（重试/熔断/超时） | 供体裸 HttpClient；商户侧本就要承受供体 6 次重试，幂等要求不变 |

## 3. 本轮未移植（明确推迟，非遗漏）

| # | 项 | 供体证据 | 归属 |
| --- | --- | --- | --- |
| 1 | sys-config 驱动 POST_BODY / POST_QUERYSTRING 与扩展参数 payOrderNotifyExtParams | PayMchNotifyService.cs:321-341,343-355 | SysConfig 切片后补 |
| 2 | 退款 / 转账通知入口 | PayMchNotifyService.cs:116-250 | S3 退款 / S6 转账切片 |
| 3 | 通知记录管理查询 API（列表/手动重发） | 供体 MchNotifyRecordController | S4 CRUD 波 |
| 4 | ReturnUrl 同步跳转签名 | PayMchNotifyService.cs:303-319 | S5 渠道 SPI |
| 5 | NotifyOrderType.Transfer 枚举值（供体 TYPE_TRANSFER_ORDER=3） | Common/Enumerator | S6 转账切片 |

## 4. 验证证据

```text
dotnet build Zentin.Payment.slnx --nologo -v q        → 0 个错误
dotnet test Zentin.Payment.slnx --nologo -v q         → 总计 44，失败 0，成功 44
dotnet ef migrations has-pending-model-changes
  --project Zentin.Payment.Infrastructure
  --startup-project Zentin.Payment.Infrastructure     → No changes（无需迁移）
```

集成测试（真实 localdb + WebApplicationFactory，假 IMchNotifySender）：
- 支付成功事件 → 签名记录落库 + outbox dispatch 行；SUCCESS 终止 + NotifyState 回写 + 终态重投幂等
- 失败尝试 → count=1 + 第二条 dispatch 行 ExpiresAt > Added（延迟 30s）+ 小写 success 终止
- 重复支付事件 → 唯一记录

## 5. AI 协作度量

- 首次通过率：Domain/Contracts/Infrastructure 代码首次编译通过；1 次包版本降级（NU1605，10.0.0→10.0.5）
- 返工轮数：2（CAP `PublishDelayAsync` headers=null 触发 NRE → 改传空字典；延迟行状态断言 `Delayed` 不成立 → 实证为 `Scheduled` + ExpiresAt）
- 人工介入次数：0
# 02 · 技术架构

> 回答：代码怎么分层、模块怎么切、渠道怎么接、数据怎么落、横切怎么做。

---

## 1. 总体形态

**模块化单体（Modular Monolith）**，单进程部署，模块间进程内调用但保持硬边界。

```
                    ┌─────────────── ApiGateway (YARP) ───────────────┐
                    │                                                │
        ┌───────────┴──┐  ┌──────────────┐  ┌──────────────┐  ┌─────┴────────┐
        │ Manager.Api  │  │ Merchant.Api │  │  Agent.Api   │  │ Payment.Api  │
        └───────┬──────┘  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘
                └────────────────┴─────────────────┴─────────────────┘
                                          │
                        ┌─────────────────▼──────────────────┐
                        │        Application（用例层）        │
                        │  Command/Query Handler · Validator │
                        └─────────────────┬──────────────────┘
                                          │
        ┌─────────────────────────────────▼─────────────────────────────────┐
        │                        Domain（领域层，零外部依赖）                 │
        │  Aggregate Root · 状态机 · 领域事件 · IRepository 端口              │
        └─────────────────────────────────▲─────────────────────────────────┘
                                          │ 实现
        ┌─────────────────────────────────┴─────────────────────────────────┐
        │                     Infrastructure（基础设施层）                    │
        │  EF Core · Repository 实现 · Outbox · 加密 · 缓存 · 渠道适配器       │
        └───────────────────────────────────────────────────────────────────┘
                                          │
              ┌───────────────────────────┼───────────────────────────┐
              ▼                           ▼                           ▼
        SQL Server                   RabbitMQ / Redis          上游支付渠道
```

依赖方向（编译期可验证）：`Api → Application → Domain`，`Infrastructure → Domain`，`Infrastructure → Application.Contracts`。**Infrastructure 不得引用 Application 实现**，否则 Application 无法编排它。

---

## 2. 分层职责

| 层 | 允许 | 禁止 |
| --- | --- | --- |
| **Domain** | 实体、值对象、枚举、领域事件、领域异常、`IRepository` 端口、领域服务 | EF Core、ASP.NET、MediatR、AutoMapper、Newtonsoft、`Microsoft.*`、引用 Application |
| **Application.Contracts** | Command/Query 契约、DTO、渠道 SPI 接口、`ApiResult` | 实现逻辑、数据库访问 |
| **Application** | Handler、Validator、编排、事务边界、DTO↔实体映射 | 直接 `new DbContext`、拼 SQL、引用 Infrastructure 具体类型 |
| **Infrastructure** | EF 配置、`DbContext`、仓储实现、Outbox、渠道适配器、缓存、加密 | 业务规则（业务规则必须在 Domain） |
| **Api** | Controller/Endpoint、认证、授权、统一响应、异常映射 | 业务逻辑、直接访问 DbContext |

### 2.1 当前偏差（必须在 P1 纠正）

`Zentin.Payment.Domain.csproj` 目前引用 `Application.Contracts` 且包含 `Microsoft.EntityFrameworkCore`、`AutoMapper`、`MediatR`、`Newtonsoft.Json`、`Microsoft.Extensions.Caching.Abstractions`。这会让"领域层零依赖"的约束失去执行力。

纠正步骤（P1 第一天做）：

1. 把 `Domain/Commands/**` 迁到 `Application.Contracts/Commands/**`。
2. 从 `Domain.csproj` 删除全部 `PackageReference` 与 `ProjectReference`。
3. 删掉 Domain 里所有 `using Microsoft.*` / `using Newtonsoft.*`。
4. 重新编译，逐个消除因此暴露的类型依赖（预计多数是 DTO 放错层）。

**不要**为了"让编译过"而把 EF 引用加回 Domain。

---

## 3. 模块边界

6 个模块，每个模块拥有自己的实体、仓储端口和 EF 配置。模块间**只能**通过 Application 端口或领域事件通信。

| 模块 | 聚合/实体 | 对外端口（示例） |
| --- | --- | --- |
| **M1 Payment** | `PayOrder`、`MchNotifyRecord` | `IPayOrderQueryPort`、`IMchNotifyPort` |
| **M2 Refund** | `RefundOrder` | `IRefundQueryPort` |
| **M3 Transfer** | `TransferOrder` | `ITransferQueryPort` |
| **M4 Division** | `MchDivisionReceiver`、`MchDivisionReceiverGroup`、`PayOrderDivisionRecord`、`PayOrderProfit` | `IDivisionPort` |
| **M5 Access** | `IsvInfo`、`AgentInfo`、`MchInfo`、`MchApp`、`MchStore`、`MchApply`、`PayInterfaceDefine`、`PayInterfaceConfig`、`MchPayPassage`、`PayWay`、`PayRateConfig`、`PayRateLevelConfig`、`QrCode`、`QrCodeShell` | `IMerchantConfigPort`、`IPayRoutePort` |
| **M6 Ledger** | `AccountBill`、`MbrInfo`、`MbrBill`、`MbrRechargeOrder`、`MbrRechargeRule`、`CheckBillBatch`、`ChannelBill`、`DiffBill` | `IAccountLedgerPort` |

规则：

- 一个模块一个 `IEntityTypeConfiguration` 命名空间；`DbContext` 可以由一个共享的 `PaymentDbContext` 承载（模块化单体的务实做法），但**禁止**模块之间直接引用对方的实体类型来写查询。
- 跨模块写操作必须走端口，不允许"顺手 join 另一张表然后更新"。
- 领域事件在模块内发布，跨模块消费方在 Application 层订阅。

> 若将来要拆服务，端口就是天然的拆分面。现在不拆，但按可拆的方式写。

---

## 4. 渠道 SPI（防腐层）

### 4.1 接口清单

**核心 7 个**（支付主链路）：

| 接口 | 职责 | 关键方法（示意） |
| --- | --- | --- |
| `IPaymentService` | 下单 | `Task<ChannelPayResult> PayAsync(ChannelPayRequest req, CancellationToken ct)` |
| `IRefundService` | 退款 | `Task<ChannelRefundResult> RefundAsync(ChannelRefundRequest req, CancellationToken ct)` |
| `IPayOrderQueryService` | 查单 | `Task<ChannelQueryResult> QueryAsync(ChannelQueryRequest req, CancellationToken ct)` |
| `IPayOrderCloseService` | 关单 | `Task<ChannelCloseResult> CloseAsync(ChannelCloseRequest req, CancellationToken ct)` |
| `ITransferService` | 转账 | `Task<ChannelTransferResult> TransferAsync(...)` |
| `IChannelNoticeService` | 支付回调 | `Task<ChannelNoticeResult> ParseAsync(ChannelNoticeContext ctx, CancellationToken ct)` |
| `IChannelRefundNoticeService` | 退款回调 | `Task<ChannelRefundNoticeResult> ParseAsync(...)` |

**扩展 4 个**：

| 接口 | 职责 |
| --- | --- |
| `ITransferNoticeService` | 转账回调 |
| `IDivisionService` | 分账 |
| `IDivisionRecordChannelNotifyService` | 分账结果回调 |
| `IChannelUserService` | 渠道用户授权/绑定（微信 openid、支付宝 userid 等） |

**接口位置**：`Zentin.Payment.Application.Contracts/Channel/`。适配器项目只引用 `Application.Contracts` 和渠道 SDK，**不引用** `Application`。

### 4.2 显式注册（替换反射）

供体做法是 `Assembly.GetTypes()` + `Activator.CreateInstance` + `GetIfCode()` 推断注册键，并 `services.BuildServiceProvider()` 建二次容器。全部废弃。

目标写法：每个渠道一个注册扩展，集中调用。

```csharp
// Infrastructure.Channel.WxPay/ServiceCollectionExtensions.cs
public static class WxPayChannelRegistration
{
    public const string IfCode = "wxpay";

    public static IServiceCollection AddWxPayChannel(this IServiceCollection services)
    {
        services.AddKeyedScoped<IPaymentService, WxPayPaymentService>(IfCode);
        services.AddKeyedScoped<IRefundService, WxPayRefundService>(IfCode);
        services.AddKeyedScoped<IPayOrderQueryService, WxPayQueryService>(IfCode);
        services.AddKeyedScoped<IPayOrderCloseService, WxPayCloseService>(IfCode);
        services.AddKeyedScoped<IChannelNoticeService, WxPayNoticeService>(IfCode);
        services.AddKeyedScoped<IChannelRefundNoticeService, WxPayRefundNoticeService>(IfCode);
        services.AddKeyedScoped<ITransferService, WxPayTransferService>(IfCode);
        return services;
    }
}
```

```csharp
// Infrastructure/Channel/ChannelRegistration.cs
public static IServiceCollection AddChannels(this IServiceCollection services)
{
    services.AddWxPayChannel();
    services.AddAliPayChannel();
    // 每接一个渠道，显式加一行。新增渠道漏注册会在启动时被工厂校验发现。
    return services;
}
```

```csharp
// Application/Channel/ChannelServiceFactory.cs
public sealed class ChannelServiceFactory<T>(IServiceProvider provider) : IChannelServiceFactory<T>
    where T : class
{
    public T Get(string ifCode) =>
        provider.GetRequiredKeyedService<T>(ifCode);
}
```

**关键差异**：注册键是编译期常量，不是运行期反射推断；找不到服务时抛异常，不吞掉返回 `default`。

### 4.3 启动期自检

注册完成后做一次校验：把 `AddChannels()` 声明的 `(interface, ifCode)` 组合与配置表 `PayInterfaceDefine` 中启用的渠道集合比对，缺失即启动失败。避免"线上某个渠道静默不可用"。

---

## 5. 订单状态机与一致性

### 5.1 状态机

```
PayOrder:
  Created ──► Paying ──► Success        （终态，退款走 RefundState）
     │           │
     │           └─────► Failed
     └───────────────► Closed

RefundOrder:
  Created ──► Processing ──► Success | Failed

TransferOrder:
  Created ──► Processing ──► Success | Failed
```

规则：

- 状态只能通过聚合方法迁移（`MarkSuccess` / `MarkPaying` / `MarkFail` / `Close` / `RecordRefund`），禁止外部直接赋值。
- 非法迁移抛领域异常，由 API 层映射为业务错误码。
- `Success` 之后不允许再改金额；退款只改 `RefundState` / `RefundAmount` / `RefundTimes`。

### 5.2 幂等

| 场景 | 幂等键 | 实现 |
| --- | --- | --- |
| 下单 | `(MchNo, MchOrderNo)` | 数据库唯一索引 + 捕获唯一键冲突后查回原单 |
| 退款 | `(MchNo, MchRefundNo)` | 同上 |
| 转账 | `(MchNo, MchOrderNo)` | 同上 |
| 支付回调 | `ChannelOrderNo` + 状态守卫 | `MarkSuccess` 前判断已是 `Success` 则直接返回成功 |
| 商户通知 | `MchNotifyRecord` 唯一约束 | 重发幂等 |

### 5.3 Outbox

领域事件不能在数据库事务内直接发 MQ（会丢或会重复）。做法：

```
BEGIN TRAN
  UPDATE PayOrder SET State = Success ...
  INSERT OutboxMessage(Type='PayOrderPaid', Payload=..., OccurredOnUtc=now)
COMMIT
        │
        ▼ 后台 HostedService / SchedulerJob 轮询
  SELECT TOP(100) * FROM OutboxMessage WHERE ProcessedOnUtc IS NULL ORDER BY Id
        │
        ├─ 投递 MQ 或直接调商户回调
        └─ 成功则 ProcessedOnUtc = now，失败则 Attempts++ 并按指数退避重试
```

`OutboxMessage` 是本项目**自建**的表（供体没有），属于 M1 Payment 模块。

---

## 6. 数据访问规则

| 规则 | 原因 |
| --- | --- |
| 一个 `PaymentDbContext`，模块共享但分区配置 | 模块化单体的务实选择；将来可拆 |
| 禁止 `IQueryable` 出 Application | 否则 Infrastructure 的查询被 API 组装，事务与性能不可控 |
| 仓储返回 `Task<T?>`、`Task<IReadOnlyList<T>>`、`Task<PagedResult<T>>` | 明确契约 |
| 分页/排序/过滤在仓储内部完成 | 避免全表加载 |
| 金额 `long`（分），费率 `decimal(20,6)` | 与供体和现有代码一致；禁止 `double` |
| 枚举存 `int`，默认值必须传枚举值本身 | `HasDefaultValue(0)` 对枚举属性会抛类型错误（已踩过） |
| 表名 PascalCase 单数，一个表名只能被一个实体类型映射 | 否则 EF 模型验证失败 |
| 每个实体一个 `IEntityTypeConfiguration`，一个实体只能有一份配置 | 两份配置会静默互相覆盖，FK 名和索引不可复现 |
| 迁移只向前，不在生产库手工改表 | 可追溯 |

### 6.1 EF 模型守卫（已落地）

`tests/Zentin.Payment.Infrastructure.Tests/PaymentDbContextModelTests.cs` 两个测试：

1. `Model_builds_without_validation_errors`：`context.Model` 能构建。
2. `Each_table_is_mapped_by_exactly_one_entity_type`：无表名冲突。

这两个测试是**准入闸门**。它们红了，`dotnet build` 再绿也不能提交。

---

## 7. 横切关注点

| 关注点 | 方案 | 归属 |
| --- | --- | --- |
| 认证 | IdentityServiceCenter 签发 JWT（OpenIddict），本仓库只验签 | P5 |
| 授权 | 权限码 + `[Authorize(Policy=...)]`，权限数据来自 IdentityServiceCenter | P5 |
| 统一响应 | `ApiResult<T> { Code, Msg, Data, TraceId }` | P0/P5 |
| 异常映射 | 领域异常 → 业务错误码；未知异常 → 500 + `TraceId` | P5 |
| 审计字段 | `CreatedOnUtc` / `ModifiedOnUtc`，由 `SaveChangesInterceptor` 自动写（已有 `AuditFieldInterceptor`） | 已有 |
| 日志 | Serilog + `TraceId`，禁止 `Console.WriteLine` | P5 |
| 缓存 | Redis，缓存商户/应用/渠道配置，**不缓存订单状态** | P2 |
| 分布式锁 | Redis 锁保护回调处理与退款并发 | P2 |
| 密钥加密 | 渠道密钥 AES-GCM 加密后落库；密钥来自配置/环境变量 | P2 |
| 响应脱敏 | 渠道密钥字段永不出现在 API 响应 | P2 |
| 限流 | 按 `AppId` 限流 | P5 |
| 健康检查 | `/health` 含 DB 与 Redis | P5 |
| 追踪 | OpenTelemetry（可延后） | P6 |

---

## 8. 平台复用边界

| 能力 | 来源 | 本仓库要做的事 |
| --- | --- | --- |
| 路由/网关 | `ApiGateway`（YARP） | 只提供正确的路由元数据 |
| 身份/令牌 | `IdentityServiceCenter`（OpenIddict） | 验签 + 权限码映射 |
| 定时任务 | `SchedulerJob`（Quartz） | 提供对账/通知重试的 Job 端点 |
| 缓存/锁/工具 | `Zenith.Extensions.*`（NuGet，`jackbeyant`） | 直接引用，不自己造 |
| 消息 | RabbitMQ | 只做 Outbox 投递方 |

**判定原则**：平台已有的一律复用。只有在平台没有且属于支付领域内的，才在本仓库实现。

---

## 9. 运行时与部署

```
                     ┌─────────────┐
  商户/上游 ────────► │  ApiGateway │
                     └──────┬──────┘
        ┌───────────────────┼───────────────────┐
        ▼                   ▼                   ▼
  Payment.Api        Manager/Merchant/Agent.Api   后台 Job
        └───────────────────┼───────────────────┘
                            ▼
                    SQL Server（主）
                    Redis（缓存/锁）
                    RabbitMQ（事件）
```

- 四个 API 宿主可同机不同端口，也可合并为单宿主多路由（练手期建议先合并，降低部署成本）。
- 迁移在启动时**不自动执行**；用命令行 `dotnet ef database update` 或部署脚本执行。
- 配置全部走环境变量，密钥不进仓库。

---

## 10. 非功能目标（练手期够用即可）

| 指标 | 目标 |
| --- | --- |
| 下单接口 p99（不含渠道往返） | < 300 ms |
| 回调处理 p99 | < 500 ms |
| 商户通知重复率 | 0（靠幂等表） |
| 订单状态非法迁移 | 0（靠状态机 + 单测） |
| 构建警告 | < 300，且新增代码零 CS8618 | 
| EF 模型守卫 | 始终绿 |
| 单切片测试覆盖率（Domain+Application） | ≥ 70% |

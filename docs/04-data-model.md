# 04 · 数据模型

> 数据库：SQL Server。ORM：EF Core 10 Code First。本文只记录**已核实**的字段；未核实的表只给定位与约束，不编字段。

---

## 1. 建模约定

| 约定 | 规则 |
| --- | --- |
| 主键 | 业务字符串主键 + `ValueGeneratedNever()`；不使用数据库自增 |
| 表名 | PascalCase 单数，与实体名一致 |
| 金额 | `bigint`，单位：分 |
| 费率 | `decimal(20,6)` |
| 时间 | `datetime2(6)`，统一 UTC；字段名带 `Utc` 或以 `Time` 结尾 |
| 枚举 | 存 `int`；`HasDefaultValue` 必须传**枚举值本身**，不能传 `0` |
| 审计 | `CreatedOnUtc` / `ModifiedOnUtc` 由 `AuditFieldInterceptor` 自动写入 |
| 软删除 | **不使用**。需要时用状态字段表达 |
| 并发 | 关键表加 `rowversion`（P3 再引入） |
| 配置 | 一个实体只能有**一份** `IEntityTypeConfiguration` |
| 表名唯一 | 一个表名只能被**一个**实体类型映射 |

### 1.1 为什么金额不用 decimal

供体与现有代码都是 `long`（分）。改成 decimal 需要改动所有已有 EF 配置、DTO 和渠道报文字段，收益只是可读性，风险远大于收益。**保持 `long` 分**，在 API 边界做元/分转换并明确字段命名（`Amount` 一律是分）。

---

## 2. 实体文件清单（36 个文件）

| 目录 | 实体 | 表 | DbSet | EF 配置 |
| --- | --- | --- | --- | --- |
| `Agent/` | `AgentInfo` | `AgentInfo` | — | ✓ |
| `CheckBill/` | `CheckBillBatch` | `CheckBillBatch` | ✓ | ✓ |
| `CheckBill/` | `ChannelBill` | `ChannelBill` | ✓ | ✓ |
| `CheckBill/` | `DiffBill` | `DiffBill` | ✓ | ✓ |
| `Device/` | `DeviceInfo` | `DeviceInfo` | ✓ | ✓ |
| `Device/` | `DeviceProviderConfig` | `DeviceProviderConfig` | ✓ | ✓ |
| `Device/` | `DeviceProviderDefine` | `DeviceProviderDefine` | ✓ | ✓ |
| `Division/` | `MchDivisionReceiver` | `MchDivisionReceiver` | ✓ | ✓ |
| `Division/` | `MchDivisionReceiverGroup` | `MchDivisionReceiverGroup` | ✓ | ✓ |
| `Finance/` | `AccountBill` | `AccountBill` | — | ✓ |
| `Isv/` | `IsvInfo` | `IsvInfo` | — | ✓ |
| `Member/` | `MbrInfo` | `MbrInfo` | — | ✓ |
| `Member/` | `MbrBill` | `MbrBill` | — | ✓ |
| `Member/` | `MbrRechargeOrder` | `MbrRechargeOrder` | — | ✓ |
| `Member/` | `MbrRechargeRule` | `MbrRechargeRule` ⚠ | — | ✓ ⚠ |
| `Merchant/` | `MbrRechargeRule` | `MbrRechargeRule` ⚠ | — | ✓ ⚠ |
| `Merchant/` | `MchInfo` | `MchInfo` | — | ✓ |
| `Merchant/` | `MchApp` | `MchApp` | — | ✓ |
| `Merchant/` | `MchApply` | `MchApply` | — | ✓ |
| `Merchant/` | `MchStore` | `MchStore` | — | — |
| `Notify/` | `MchNotifyRecord` | `MchNotifyRecord` | ✓ | ✓ |
| `Order/` | `PayOrder` | `PayOrder` | ✓ | ✓ |
| `Order/` | `RefundOrder` | `RefundOrder` | ✓ | ✓ |
| `Order/` | `OrderSnapshot` | `OrderSnapshot` | ✓ | ✓ |
| `Pay/` | `PayOrder` ⚠ 重复 | — | — | — |
| `Pay/` | `PayWay` | `PayWay` | ✓ | ✓ |
| `Pay/` | `PayInterfaceDefine` | `PayInterfaceDefine` | ✓ | ✓ |
| `Pay/` | `PayInterfaceConfig` | `PayInterfaceConfig` | ✓ | ✓ |
| `Pay/` | `PayRateConfig` | `PayRateConfig` | ✓ | ✓ |
| `Pay/` | `PayRateLevelConfig` | `PayRateLevelConfig` | — | ✓ |
| `Pay/` | `MchPayPassage` | `MchPayPassage` | ✓ | ✓ |
| `Pay/` | `PayOrderDivisionRecord` | `PayOrderDivisionRecord` | — | ✓ |
| `Pay/` | `PayOrderProfit` | `PayOrderProfit` | — | ✓ |
| `Pay/` | `TransferOrder` | `TransferOrder` | ✓ | ✓ |
| `QrCode/` | `QrCode` | `QrCode` | ✓ | ✓ |
| `QrCode/` | `QrCodeShell` | `QrCodeShell` | ✓ | ✓ |

⚠ 见第 5 节"已知问题"。

统计：显式 DbSet 20 个；无 DbSet 的实体 15 个；无 EF 配置的实体 1 个（`MchStore`）；重复实体 2 处（`PayOrder`、`MbrRechargeRule`）。

---

## 3. 核心表设计

### 3.1 `PayOrder` 支付订单

主键 `PayOrderId`（30，业务生成，非自增）。已核实的字段：

| 分组 | 字段 | 类型/长度 | 说明 |
| --- | --- | --- | --- |
| 标识 | `PayOrderId` | nvarchar(30) PK | 平台单号 |
| 租户 | `MchNo` / `MchName` | 64 / 64，必填 | 商户 |
| 租户 | `MchShortName` | 32 | |
| 租户 | `AgentNo` / `AgentName` / `AgentShortName` | 64 / 64 / 32 | 代理商快照 |
| 租户 | `IsvNo` / `IsvName` / `IsvShortName` | 64 / 64 / 32 | ISV 快照 |
| 应用 | `AppId` / `AppName` | 64 / 64，`AppId` 必填 | |
| 门店 | `StoreId` / `StoreName` | bigint? / 64 | |
| 码牌 | `QrcId` | 64 | |
| 商户类型 | `MchType` | int，必填 | |
| 商户单号 | `MchOrderNo` | 64，必填 | 幂等键之一 |
| 渠道 | `IfCode` | 20 | 渠道代码 |
| 支付方式 | `WayCode` / `WayType` | 20 / 20，必填 | |
| 金额 | `Amount` | bigint，必填 | 分 |
| 费率 | `MchFeeRate` | decimal(20,6)，必填 | |
| 费率描述 | `MchFeeRateDesc` | 128 | |
| 手续费 | `MchFeeAmount` / `MchOrderFeeAmount` | bigint | 分 |
| 币种 | `Currency` | 3，默认 `CNY` | |
| 状态 | `State` | int（`PayOrderStatus`） | |
| 通知 | `NotifyState` | bit，默认 0 | 商户是否已收到通知 |
| 退款 | `RefundState` / `RefundTimes` / `RefundAmount` | int / int / bigint | |
| 分账 | `DivisionMode` / `DivisionState` / `DivisionLastTime` | int / int / datetime2(6) | |
| 请求方 | `ClientIp` | 32 | |
| 商品 | `Subject` / `Body` | 64 / 256，必填 | |
| 备注 | `SellerRemark` / `BuyerRemark` | 256 / 256 | |
| 渠道回执 | `ChannelMchNo` / `ChannelIsvNo` / `ChannelExtra` / `ChannelUser` / `ChannelOrderNo` | 64 / 64 / 512 / 64 / 64 | |
| 平台单号 | `PlatformOrderNo` / `PlatformMchOrderNo` | 64 / 64 | |
| 错误 | `ErrCode` / `ErrMsg` | 128 / 256 | |
| 扩展 | `ExtParam` | 128 | |
| 回调 | `NotifyUrl` / `ReturnUrl` | 128 / 128 | |
| 时间 | `ExpiredTime` / `SuccessTime` | datetime2(6) | |

索引与约束：

- `UNIQUE (MchNo, MchOrderNo)` → `UniMchNoMchOrderNo`（幂等）
- `INDEX (CreatedOnUtc)` → `IdxCreatedOnUtc`
- `INDEX (State)` → `IdxState`
- `FK RefundOrder.PayOrderId → PayOrder.PayOrderId`，`ON DELETE RESTRICT`，约束名 `FKRefundOrderPayOrderId`

导航：`RefundOrders`、`ProfitRecords`、`DivisionRecords`。

> 注意：`SellerRemark` / `BuyerRemark` / `PlatformOrderNo` 等字段在供体中是后加的，P1 只实现必需字段，其余保留但暂不写入。

### 3.2 `RefundOrder` 退款订单

主键 `RefundOrderId`（30）。已核实字段：

`PayOrderId`(30, FK, 必填)、`ChannelPayOrderNo`(64)、商户/代理商/ISV/应用/门店快照（同 PayOrder 结构）、`MchType`、`MchRefundNo`(64, 必填)、`WayCode`(20)、`WayType`(20)、`IfCode`(20)、`PayAmount`(bigint)、`RefundAmount`(bigint)、`RefundFeeAmount`(bigint)、`Currency`(3, 默认 CNY)、`State`（`RefundOrderStatus`）、`ClientIp`(32)、`RefundReason`(256, 必填)、`ChannelOrderNo`(32)、`ErrCode`(128)、`ErrMsg`(2048)、`ChannelExtra`(512)、`NotifyUrl`(128)、`ExtParam`(64)、`SuccessTime`、`ExpiredTime`。

索引：

- `UNIQUE (MchNo, MchRefundNo)` → `UniMchNoMchRefundNo`（幂等）
- `INDEX (PayOrderId)` → `IdxPayOrderId`
- `INDEX (State)` → `IdxState`

导航：`PayOrder`（必填）。FK 由 `PayOrderEntityTypeConfiguration` 单向声明，`RefundOrder` 配置中**不再重复声明**。

### 3.3 `TransferOrder` 转账订单

主键 `TransferId`（32）。字段以现有实体为准，P1 实现时逐字段核对供体 `AGooday.AgPay.Domain/Models/TransferOrder.cs` 后补全本文。

约束要点：`(MchNo, MchOrderNo)` 唯一；无 `PayOrderId` 关联。

### 3.4 `MchNotifyRecord` 商户通知

用于回调重试。关键字段（P1 实现时核定）：`NotifyId`、`MchNo`、`AppId`、业务类型（支付/退款/转账）、业务单号、回调 URL、请求体、响应体、重试次数、下次重试时间、状态。

索引：`INDEX (Status, NextRetryTime)` 供后台扫描；`UNIQUE (BizType, BizOrderNo, NotifyType)` 防重复通知。

### 3.5 `PayOrderDivisionRecord` / `PayOrderProfit`

分账记录与利润记录，挂在 `PayOrder` 的导航集合上。P3 实现。聚合根是 `PayOrder`，这两个是被聚合管理的子实体，**不应有独立仓储写入口**。

### 3.6 配置类表

`PayWay`、`PayInterfaceDefine`、`PayInterfaceConfig`、`MchPayPassage`、`PayRateConfig`、`PayRateLevelConfig` 共同构成路由与计费配置：

```
WayCode ──► PayWay（支付方式字典）
   │
   ▼
MchPayPassage（商户开了哪些支付方式，指向哪个 IfCode）
   │
   ▼
PayInterfaceConfig（该商户在该渠道的参数，密钥加密存储）
   │
   ▼
PayInterfaceDefine（渠道元数据：支持哪些动作、参数模板）

计费：MchInfo / AgentInfo / IsvInfo ──► PayRateConfig ──► PayRateLevelConfig
```

约束：`MchPayPassage` 的 `(MchNo, AppId, WayCode)` 唯一；`PayInterfaceConfig` 的 `(MchNo, AppId, IfCode)` 唯一（以实际配置为准）。

---

## 4. 状态机

### 4.1 `PayOrderStatus`

```
Created ──► Paying ──► Success
   │           │
   │           └──────► Failed
   └──────────────────► Closed
```

| 迁移 | 触发 | 方法 |
| --- | --- | --- |
| → `Paying` | 已调用渠道、等待用户支付 | `MarkPaying(ifCode, channelExtra)` |
| → `Success` | 渠道回调/主动查询成功 | `MarkSuccess(channelOrderNo, paidAmount, channelExtra)` |
| → `Failed` | 下单失败 | `MarkFail(errCode, errMsg)` |
| → `Closed` | 超时关单/用户关单 | `Close(reason)` |

不变量：

- `Success` / `Closed` 不可再迁移。
- `MarkSuccess` 必须幂等：已是 `Success` 直接返回。
- `Success` 后金额不可变。

### 4.2 `RefundOrderStatus`

```
Created ──► Processing ──► Success | Failed
```

### 4.3 `RefundState`（挂在 PayOrder 上）

`NoRefund` → `PartialRefund` → `AllRefund`。由 `RecordRefund(amount)` 计算，不手工赋值。

### 4.4 `DivisionState`

`Pending` → `Processing` → `Finished` / `Failed`（P3 核定枚举成员）。

---

## 5. 已知问题与处置

| # | 问题 | 影响 | 处置 | 阶段 |
| --- | --- | --- | --- | --- |
| DM-01 | `HasConversion<byte>()` 后 `HasDefaultValue((byte)X)` 类型不匹配 | EF 模型建不起来 | 改为 `HasDefaultValue(EnumValue)`；已在工作树修复 | P0 |
| DM-02 | `bool NotifyState` 配 `HasDefaultValue(0)` | 同上 | 改为 `HasDefaultValue(false)`；已修复 | P0 |
| DM-03 | `MbrRechargeRule` 两份实体都映射 `MbrRechargeRule` 表 | EF 模型验证失败 | 二选一：保留 `Member/` 版本（业务语义为会员充值规则），把 `Merchant/` 版本被使用的成员并入后删除；反之亦然。必须只剩一份配置 | P0 |
| DM-04 | `PayOrder` 两份：`Order/`（行为少，被 DbContext 使用）与 `Pay/`（有 `MarkPaying`/`MarkFail`/`RecordRefund`/`UpdateNotifyState` 与 `IReadOnlyCollection` 封装，未被使用） | 行为丢失或误改死代码 | 以 `Pay/` 版本为行为来源，合并进 `Order/` 命名空间，删除 `Pay/` 版本。合并后需为 `IReadOnlyCollection` 配 backing field（`builder.Navigation(...).UsePropertyAccessMode(PropertyAccessMode.Field)`） | P0-P1 |
| DM-05 | `MchStore` 无 EF 配置 | 无法持久化 | P2 补 `MchStoreEntityTypeConfiguration` | P2 |
| DM-06 | 15 个实体无 DbSet | 无法查询，且隐式进模型易撞名 | P1-P3 按切片补 DbSet；暂不用的实体在配置里显式忽略或移出模型 | P1 |
| DM-07 | 12 个实体无仓储 | 无法访问 | 按切片补 `I<X>Repository` + 实现 | P1-P3 |
| DM-08 | 供体为 MySQL，本项目为 SQL Server | 列类型差异 | EF 配置已用 `datetime2(6)`/`bigint`/`nvarchar`，以本仓库为准，不照抄供体类型 | 全程 |

### 5.1 `MbrRechargeRule` 处置建议

两份的差别：

- `Member/`：133 行，`sealed`，私有构造 + `Create` 工厂，封装完整。
- `Merchant/`：24 行，公开无参构造，贫血，但被 `MchInfo.RechargeRules` 引用（同命名空间优先解析）。

建议：**保留 `Member/` 的完整实现**，把 `Merchant/MchInfo` 的导航改为显式引用 `Domain.Entities.Member.MbrRechargeRule`，删除 `Merchant/MbrRechargeRule.cs` 与 `EntityTypeConfiguration/Merchant/MbrRechargeRuleEntityTypeConfiguration.cs`。理由：充值规则的主体是会员，不是商户；商户只是"提供规则"。

---

## 6. 迁移策略

| 规则 | 说明 |
| --- | --- |
| 只向前 | 已应用的迁移不修改、不删除 |
| 一个切片一个迁移 | 迁移名 `Add<Slice>`，如 `AddOutbox`、`AddMchStore` |
| 基线迁移 | P0 生成 `InitialCreate`，覆盖当前可构建的全部表 |
| 不自动迁移 | 启动时不执行 `Database.Migrate()`；用命令行或部署脚本 |
| 开发库重置 | 提供 `scripts/reset-db.ps1`（删库 + `database update`），不手工改表 |
| 种子数据 | 用 `HasData` 仅限字典表（`PayWay`、`PayInterfaceDefine`），业务数据不种子 |

生成迁移：

```bash
dotnet ef migrations add <Name> \
  --project Zentin.Payment.Infrastructure \
  --startup-project Zentin.Payment.Api \
  --output-dir Migrations
```

应用迁移：

```bash
dotnet ef database update \
  --project Zentin.Payment.Infrastructure \
  --startup-project Zentin.Payment.Api
```

**每次生成迁移后必须**：跑一次 EF 模型守卫测试 + 检查生成的 `Up()` 是否包含预期表/索引。迁移脚本是代码，也要 review。

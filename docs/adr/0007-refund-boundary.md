# ADR-0007 · RefundOrder 退款边界与退款路径统一

- 状态：已采纳
- 日期：2026-11-24
- 供体：`D:\Projects\agpayplus` @ `ba61fea9`（只读）

## 背景

退款切片（S3）开工前需敲定 `RefundOrder` 的聚合边界与退款路径，否则会边写边改
领域不变量。供体退款链路已全量研读：

- 申请入口：`Payment.Api/Controllers/Refund/RefundOrderController.cs`
- 状态服务：`Application/Services/RefundOrderService.cs`
- 回调处理：`Components.Third/Services/RefundOrderProcessService.cs`

Zentin 现状：`RefundOrder` 聚合已建模（`Create/MarkProcessing/MarkSuccess/MarkFailed/Close`），
但仍是 `BaseEntity`（未升聚合根）、`MarkSuccess/MarkFailed` 对重复回调抛异常、
`RefundSuccessDomainEvent` 字段命名不规范、`PayOrder.RecordRefund` 未扣手续费且无人调用。

## 决策

### 1. RefundOrder 为独立聚合根，跨聚合回写在单一 UoW 事务内

- 与 `docs/02-technical-architecture.md` 既定一致（M2 Refund 独立模块）。
- `PayOrder` 持有的 `RefundOrders` 集合导航是读模型，不承载退款不变量。
- 退款成功同时改 `RefundOrder` 与 `PayOrder`，经同一 `SaveChangesAsync` 落库
  （对齐供体 `UpdateIng2SuccessAsync` 内先改退款单再改订单，RefundOrderService.cs:178-192）。
- 领域事件出站仍走 outbox（ADR-0006），退款成功发 `RefundSuccessDomainEvent`。

### 2. 退款金额「成功时扣减」，不在创建时预留

- 供体证据：`PayOrder.RefundAmount/RefundTimes/RefundState/State/MchFeeAmount` 只在
  `UpdateRefundAmountAndCountAsync` 更新（RefundOrderService.cs:201-222），该函数仅在
  `UpdateIng2SuccessAsync`（退款成功）时调用。
- 创建时只校验可退余额，不写 `PayOrder`（RefundOrderController.cs:82-107）。
- 后果：退款失败无需回滚 `PayOrder`；可退余额 = `Amount - 已成功退款额`，
  在途退款额由「单订单串行退款」约束，无需预留。

### 3. 单订单串行退款，用过滤唯一索引兜底并发

- 供体用 `IsExistRefundingOrderAsync` 查询 `STATE_ING` 串行化
  （RefundOrderRepository.cs:21-24），但存在并发窗口（两笔并发退款可同时通过检查）。
- Zentin 退款创建与渠道调用**拆分**（见决策 4），`Created` 态会持久化，
  故串行守卫必须覆盖 `Created + Processing`。
- 决策：保留应用层守卫（对齐供体），并加 SQL Server **过滤唯一索引**彻底消除并发超退：

  ```sql
  CREATE UNIQUE INDEX UQ_RefundOrder_InFlight ON RefundOrder (PayOrderId)
  WHERE State IN (0 /*Created*/, 1 /*Processing*/);
  ```

  违反者触发唯一键冲突 → 映射为「存在在途退款」业务错误。

### 4. 退款创建与渠道调用拆分（异步化），渠道端口留 S5

- 供体是同步：controller 内建单、同步调渠道、同步处理结果（RefundOrderController.cs:140-149）。
- Zentin 采用 CAP 事件驱动：
  1. `CreateRefundOrderCommand`：校验 + 建单（`Created`）→ 出站 `RefundOrderCreated` 事件；
  2. 渠道调用由 `IRefundService` 端口消费（S5 渠道 SPI 落地）；
  3. 渠道结果/回调 → `ConfirmRefundSuccess/FailCommand` 驱动状态机 + 回写 `PayOrder`。
- 当前 S3 只做「创建命令 + 状态机 + 回调命令 + 领域事件出站（成功→商户通知复用 S2 链）」，
  渠道调用端口 `IRefundService` 在 S5 落地（docs/02 已声明该端口）。

### 5. 手续费退还：S3 阶段退 0，S5 渠道落地后按渠道规则计算

- 供体：`RefundFeeAmount = refundService.CalculateFeeAmount(RefundAmount, payOrder)`
  （RefundOrderController.cs:215），成功后 `PayOrder.MchFeeAmount -= RefundFeeAmount`
  （RefundOrderService.cs:217）。
- `CalculateFeeAmount` 是渠道 SPI 的默认实现，依赖渠道费率规则，S5 才落地。
  S3 阶段 `RefundFeeAmount = 0`（不退手续费），保留字段与扣减调用点；
  S5 渠道落地后按渠道规则计算。理由：铁律 10 不猜语义。

### 6. 利润重结算与账单推迟到分润/账单切片

- 供体退款成功重算分润 + 生成账户账单（RefundOrderProcessService.cs:79-137）。
- Zentin 分润/账单是 S6+ 切片，订阅 `RefundSuccessDomainEvent` 后处理。S3 只出事件。

### 7. 幂等

- 回调幂等：仅 `Processing` 可转 `Success/Failed`，重复回调幂等跳过
  （对齐供体 `State != STATE_ING → return false`，RefundOrderService.cs:173-176、234-237）。
- 创建幂等：唯一索引 `(MchNo, MchRefundNo)` 已配置（`UniMchNoMchRefundNo`），
  对齐 `docs/02` 5.2。

### 8. 状态机统一（补上 Closed）

`docs/02` 状态机图缺 `Closed` 态；供体有超时关闭
（`UpdateOrderExpiredAsync`，`STATE_CLOSED`，RefundOrderService.cs:277-287，
默认 `ExpiredTime = now + 2h`，RefundOrderController.cs:226）。统一为：

```
RefundOrder:
  Created ──► Processing ──► Success | Failed
     │             │
     └─────────────┴────► Closed（过期关闭）
```

### 9. 领域事件命名规范化

`RefundSuccessDomainEvent` 现有字段 camelCase（`refundOrderId/payOrderId/refundAmount`），
违反 C# 命名且与 `PayOrderPaidDomainEvent` 不一致。统一为 PascalCase record。

## 退款路径统一（本 ADR 配套代码对齐）

| # | 现有 | 改为 | 依据 |
| --- | --- | --- | --- |
| 1 | `RefundOrder : BaseEntity` | `AggregateRoot` | 决策 1 |
| 2 | `MarkSuccess/MarkFailed` 重复回调抛异常 | 幂等跳过（已 Success/Failed/Closed 直接返回） | 决策 7 |
| 3 | `PayOrder.RecordRefund(long refundAmount)` 不扣手续费 | `RecordRefund(long refundAmount, long refundFeeAmount)` 扣 `MchFeeAmount` | 决策 2/5 |
| 4 | `RefundSuccessDomainEvent` camelCase | PascalCase | 决策 9 |
| 5 | 无在途退款索引 | `RefundOrder` 加过滤唯一索引（迁移） | 决策 3 |
| 6 | `docs/02` 状态机缺 Closed | 补 Closed 态 | 决策 8 |

## 后果

- 正向：并发超退被数据库级禁止；退款失败零回滚；渠道/分润/通知解耦，各切片可独立推进。
- 负向：异步化后 `Created` 态跨请求存在，串行守卫与索引必须覆盖 `Created`（与供体仅查
  `ING` 不同，是 Zentin 有意差异，写入 legacy-parity）。
- 迁移：新增过滤唯一索引，需一条 EF 迁移（S3 落地时生成）。

## 替代方案

- **退款金额创建时预留**：可退余额更早锁定，但要处理退款失败/超时释放预留的补偿路径，
  复杂度高；供体不用预留，否决。
- **RefundOrder 并入 PayOrder 聚合**：退款回调要加载整单所有退款，放大锁粒度，
  且 docs/02 已定独立模块，否决。
- **退款渠道调用保持同步**（照搬供体）：与 CAP outbox 事件模型冲突，渠道异常会让
  商户 API 同步挂起，否决。
- **S3 就落地 CalculateFeeAmount 手续费**：渠道费率规则属 S5 渠道 SPI，现在写是猜语义，
  违反铁律 10，否决。
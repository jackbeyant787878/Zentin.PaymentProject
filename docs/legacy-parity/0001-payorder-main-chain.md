# Legacy Parity 0001 · PayOrder 支付主链

- 日期：2026-11-24
- 供体基线：`D:\Projects\agpayplus` @ `ba61fea9`（只读）
- 切片范围：创建 → 支付中 → 成功/失败确认 → 关单 + 查询；CAP outbox 原子发布 `zentin.payorder.paid`

以下路径均相对供体 `aspnet-core/src/`。

## 1. 语义对齐项（与供体一致）

| 规则 | 供体证据 | Zentin 落点 |
| --- | --- | --- |
| 仅 ING→SUCCESS | `AGooday.AgPay.Application/Services/PayOrderService.cs:200-203` | `PayOrder.MarkSuccess` 仅允许 Paying |
| 仅 ING→FAIL | `PayOrderService.cs:250-253` | `PayOrder.MarkFail` 仅允许 Paying |
| 仅 INIT/ING 可关单 | `AGooday.AgPay.Payment.Api/Controllers/PayOrder/CloseOrderController.cs:64-67` | `PayOrder.Close` 仅允许 Created/Paying |
| 手续费四舍五入到分 | `AGooday.AgPay.Common/Utils/AmountUtil.cs:20`（`(long)decimal.Round(amount*rate, 0, AwayFromZero)`） | `CreatePayOrderCommandHandler` |
| 订单号 "P"+雪花 | `AGooday.AgPay.Common/Utils/SeqUtil.cs:25-33` | `CreatePayOrderCommandHandler` |
| 商户单号防重 | `AGooday.AgPay.Payment.Api/Controllers/PayOrder/AbstractPayOrderController.cs:133` | Handler `IsExistByMchOrderNoAsync` + 唯一索引 `UniMchNoMchOrderNo` 兜底 |
| 通知地址 URL 校验 | `AbstractPayOrderController.cs:138-145` | `CreatePayOrderCommandValidator`（http/https 绝对地址） |
| 成功写渠道单号/用户/平台单号/成功时间 | `PayOrderService.cs:205-211` | `MarkSuccess(channelOrderNo, channelUser, platformOrderNo)` |

## 2. 有意差异（增强，非疏忽）

| 差异 | 供体行为 | Zentin 行为 | 理由 |
| --- | --- | --- | --- |
| 状态并发保护 | 读-检-写，无 WHERE State 条件（`PayOrderService.cs:166-170`） | `State` 配 `IsConcurrencyToken`，UPDATE 带 `WHERE State=@原值` | 消除并发回调/关单竞态；不改表结构，无迁移 |
| 重复回调 | 控制器层忽略非 ING 订单（`ChannelNoticeController.cs:248-259`），服务层返回 false | 聚合层幂等 no-op（已 Success 再 MarkSuccess 直接返回，不重复发事件） | 幂等下沉到聚合，任何入口都安全 |
| 落库→发 MQ | 双写无事务，catch 吞异常丢消息（`Components.Third/Services/PayOrderProcessService.cs:66-71`） | CAP outbox 与业务同事务（ADR-0006） | 资金链路不可丢消息 |
| 成功时间 | `DateTime.Now`（`PayOrderService.cs:211`） | `DateTime.UtcNow` | 全库 UTC 基准 |
| MarkSuccess 签名 | 收 channelMchNo/channelIsvNo/platformMchOrderNo 等 7 参 | 本轮仅 channelOrderNo/channelUser/platformOrderNo | 其余渠道字段随渠道 ACL 切片接入；原 `paidAmount` 参数收而不用，已删除 |

## 3. 本轮未移植（明确推迟，非遗漏）

| 项 | 供体证据 | 归属 |
| --- | --- | --- |
| 渠道适配器（验签/报文/调渠道） | `AbstractPayOrderController.cs:231` paymentService.PayAsync | 渠道 SPI 切片（ADR-0003） |
| 记账 GenAccountBillAsync | `PayOrderProcessService.cs:42`、`AccountBillService.cs:30-56` | AccountBill 切片（清单 #21） |
| 自动分账 + 延迟 60s MQ | `PayOrderProcessService.cs:55-72` | 分账切片（清单 #17-19） |
| 商户通知记录 + 6 次延迟重试（0/30/60/90/120/150s） | `PayMchNotifyService.cs:45-110`、`Base.Api/MQ/PayOrderMchNotifyMQReceiver.cs:117-119` | MchNotifyRecord 切片（清单 #4）；CAP 消息已发布，消费者待接 |
| 超时订单批量关单 | `PayOrderService.cs:301-311`（ExecuteUpdate WHERE State∈{0,1} AND ExpiredTime<now） | 定时任务切片 |
| QR_CASHIER 预下单费率=0 后补快照 | `AbstractPayOrderController.cs:318`、`PayOrderService.cs:178-180` | 渠道切片（MarkPaying 目前只落 IfCode） |
| 商户/应用/通道上下文校验 | `AbstractPayOrderController.cs:148-187` | MchInfo/MchApp/MchPayPassage 切片（清单 #6/#9/#10） |

## 4. 验证证据

```
dotnet build Zentin.Payment.slnx            → 0 错误
dotnet test  Zentin.Payment.slnx            → 32/32（含 7 聚合单测 + 3 localdb 集成测试）
dotnet ef migrations has-pending-model-changes → No changes（并发令牌无迁移需求）
集成测试断言：Cap.Published 中 zentin.payorder.paid 行与订单状态同事务写入
```
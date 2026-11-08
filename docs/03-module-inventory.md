# 03 · 模块清单与进度

> 这张表是每周复盘的唯一依据。状态列必须真实，不允许"差不多完成了"。

图例：`✓` 已有且可用 · `△` 有但有问题/不完整 · `—` 缺失 · `n/a` 不适用

---

## 1. 业务切片（22 个）

来源：供体 `AGooday.AgPay.Application/Services` 的 35 个服务，剔除 12 个 `Sys*`/`Auth*`（交给 IdentityServiceCenter）与 1 个基类 `AgPayService`，剩 22 个业务切片。

| # | 切片 | 主要聚合 | 实体 | EF 配置 | DbSet | 仓储 | Handler | API | 阶段 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | PayOrder 支付订单 | `PayOrder` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 2 | RefundOrder 退款 | `RefundOrder` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 3 | TransferOrder 转账 | `TransferOrder` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 4 | MchNotifyRecord 商户通知 | `MchNotifyRecord` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 5 | PayWay 支付方式 | `PayWay` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 6 | MchPayPassage 商户通道 | `MchPayPassage` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 7 | PayInterfaceConfig 渠道参数 | `PayInterfaceConfig` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 8 | PayInterfaceDefine 渠道定义 | `PayInterfaceDefine` | ✓ | ✓ | ✓ | ✓ | — | — | P1 |
| 9 | MchInfo 商户 | `MchInfo` | ✓ | ✓ | — | — | — | — | P2 |
| 10 | MchApp 商户应用 | `MchApp` | ✓ | ✓ | — | — | — | — | P2 |
| 11 | MchStore 门店 | `MchStore` | ✓ | — | — | — | — | — | P2 |
| 12 | IsvInfo 服务商 | `IsvInfo` | ✓ | ✓ | — | — | — | — | P2 |
| 13 | AgentInfo 代理商 | `AgentInfo` | ✓ | ✓ | — | — | — | — | P2 |
| 14 | PayRateConfig 费率 | `PayRateConfig`、`PayRateLevelConfig` | ✓ | ✓ | ✓ / — | ✓ / ✓ | — | — | P2 |
| 15 | QrCode 二维码 | `QrCode` | ✓ | ✓ | ✓ | ✓ | — | — | P2 |
| 16 | QrCodeShell 码牌 | `QrCodeShell` | ✓ | ✓ | ✓ | ✓ | — | — | P2 |
| 17 | MchDivisionReceiver 分账接收方 | `MchDivisionReceiver` | ✓ | ✓ | ✓ | ✓ | — | — | P3 |
| 18 | MchDivisionReceiverGroup 接收组 | `MchDivisionReceiverGroup` | ✓ | ✓ | ✓ | ✓ | — | — | P3 |
| 19 | PayOrderDivisionRecord 分账记录 | `PayOrderDivisionRecord` | ✓ | ✓ | — | ✓ | — | — | P3 |
| 20 | PayOrderProfit 订单利润 | `PayOrderProfit` | ✓ | ✓ | — | — | — | — | P3 |
| 21 | AccountBill 账户流水 | `AccountBill` | ✓ | ✓ | — | — | — | — | P3 |
| 22 | Statistic 统计 | 聚合查询 | n/a | n/a | n/a | n/a | — | — | P4 |

**关键事实**：

- **Handler 与 API 全为 0**。DTO 有 100+ 个，但没有一个用例端到端打通。
- `PayOrder`/`RefundOrder` 的"实体 ✓"是在**修正重复之后**才成立，详见 `04-data-model.md` 第 5 节。
- `MchStore` 没有任何 EF 配置，属 P2 补齐项。
- `PayOrderDivisionRecord`、`PayOrderProfit`、`AccountBill` 有配置无 DbSet，由 `ApplyConfigurationsFromAssembly` 隐式进入模型。

### 1.1 阶段划分

| 阶段 | 切片 | 目标 |
| --- | --- | --- |
| P1 支付主链 | 1-8 | 一笔订单从下单到回调通知全通 |
| P2 商户与接入配置 | 9-16 | 商户自助配置渠道、费率、码牌 |
| P3 分账与账务 | 17-21 | 支付成功后可分账、可对账流水 |
| P4 统计与辅助 | 22 | 报表 |

---

## 2. 非供体切片（Zentin 自有，延后）

这些在供体 22 个业务服务里没有对应，属于扩展或重构产物，**不占用 P1-P4 排期**：

| 切片 | 实体 | 说明 | 建议阶段 |
| --- | --- | --- | --- |
| 对账 | `CheckBillBatch`、`ChannelBill`、`DiffBill` | 实体与配置齐备，无服务 | P6 |
| 设备 | `DeviceProviderDefine/Config`、`DeviceInfo` | 实体与配置齐备，无服务 | 延后 |
| 会员 | `MbrInfo`、`MbrBill`、`MbrRechargeOrder`、`MbrRechargeRule` | 实体齐备，无服务；`MbrRechargeRule` 有重复 | P6 |
| 商户申请 | `MchApply` | 实体与配置齐备 | P6 |
| 订单快照 | `OrderSnapshot` | 实体、配置、仓储、DbSet 齐备 | P3 |
| Outbox | `OutboxMessage`（待建） | 本项目自建，见 `02-technical-architecture.md` 5.3 | P1 |

---

## 3. 渠道矩阵（15 个）

供体 `AGooday.AgPay.Components.Third/Channel` 下 15 个渠道，共 354 个 `.cs`。已整体移出 slnx，位于 `Zentin.Payment.ThirdPartyAdapters/`，仅作语料。

移植规则（不可协商）：

1. **禁止就地修复**该目录的编译错误。修复等于把旧耦合搬进新架构。
2. 每个渠道新建独立项目 `Zentin.Payment.Infrastructure.Channel.<IfCode>`，只引用 `Application.Contracts` + 渠道 SDK。
3. 保留签名/验签/报文拼装原样。
4. 提取报文 fixture 前必须脱敏（密钥、证书、卡号、签名体只留摘要）。

| 批次 | 渠道 | IfCode | SDK | 优先级 | 状态 |
| --- | --- | --- | --- | --- | --- |
| B1（试点） | 微信支付 | `wxpay` | `SKIT.FlurlHttpClient.Wechat.TenpayV3` | 最高 | 未开始 |
| B2 | 支付宝 | `alipay` | `AlipaySDKNet.Standard` | 高 | 未开始 |
| B2 | 通联 | `allinpay` | 直连 HTTP | 中 | 未开始 |
| B2 | 银联商务 | `umspay` | 直连 HTTP | 中 | 未开始 |
| B2 | 拉卡拉 | `lklpay` | 直连 HTTP | 中 | 未开始 |
| B2 | 银盛 | `ysfpay` | 直连 HTTP | 中 | 未开始 |
| B3 | 斗拱 | `dgpay` | 直连 HTTP | 低 | 未开始 |
| B3 | 海科融通 | `hkrtpay` | 直连 HTTP | 低 | 未开始 |
| B3 | 嘉联 | `jlpay` | 直连 HTTP | 低 | 未开始 |
| B3 | 乐刷 | `lcswpay` | 直连 HTTP | 低 | 未开始 |
| B3 | 乐刷（Les） | `lespay` | 直连 HTTP | 低 | 未开始 |
| B3 | 随行付 | `sxfpay` | 直连 HTTP | 低 | 未开始 |
| B3 | 小新 | `xxpay` | 直连 HTTP | 低 | 未开始 |
| B3 | 银盛（Yse） | `ysepay` | 直连 HTTP | 低 | 未开始 |
| B4 | PayPal | `pppay` | `PayPalServerSDK` | 低 | 未开始 |

**批次原则**：B1 只做微信，用来打磨 SPI 与切片模板；B2 一次性验证"模板可复制"；B3 是重复劳动，用于练手速；B4 验证异构 SDK。

**每渠道 DoD**：下单 / 回调 / 查单 / 关单 / 退款 / 退款回调 六个动作各有至少 1 个报文 fixture 测试 + 1 个集成测试（可用渠道沙箱或录制回放）。

---

## 4. 平台能力清单（复用，不自建）

| 能力 | 来源 | 本仓库需要提供 |
| --- | --- | --- |
| API 网关/路由 | ApiGateway（YARP） | 路由元数据 |
| 身份认证 | IdentityServiceCenter（OpenIddict） | 验签配置 |
| 权限模型 | IdentityServiceCenter | 权限码常量 |
| 定时任务 | SchedulerJob（Quartz） | Job 触发端点 |
| 缓存/锁/工具库 | `Zenith.Extensions.*` NuGet | 引用即可 |
| 消息 | RabbitMQ | Outbox 投递 |
| 日志 | Serilog | 配置 + TraceId |

**不要**在本仓库实现上表任何一项。发现平台能力不足时，先记 ADR，再决定是提需求还是本地兜底。

---

## 5. 宿主清单

| 宿主 | 面向 | 认证方式 | 现状 | 阶段 |
| --- | --- | --- | --- | --- |
| `Zentin.Payment.Api` | 商户后端机调 + 渠道回调 | AppId + 签名 / 渠道验签 | 默认模板 | P1 |
| `Zentin.Payment.Merchant.Api` | 商户控制台 | JWT + 权限码 | 默认模板 | P2 |
| `Zentin.Payment.Agent.Api` | 代理商控制台 | JWT + 权限码 | 默认模板 | P4 |
| `Zentin.Payment.Manager.Api` | 平台/ISV 运营 | JWT + 权限码 | 默认模板 | P2 |

练手期建议：先只跑 `Payment.Api`，其余三个保持空壳，等 P2 再启用。

---

## 6. 每周更新模板

在每次周复盘时，把下面这段追加到本文件末尾（或维护在独立周报里）：

```
## 周报 YYYY-Www

- 完成切片：
- 新增渠道：
- 未达预期的项及原因：
- 本周 AI 度量：首次通过率 __% / 平均返工 __ 轮 / 人工介入 __ 次
- 新增/更新 ADR：
- 下周目标（最多 3 项）：
```

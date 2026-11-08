# 01 · 产品架构

> 回答三个问题：这个系统给谁用、它负责什么、它明确不负责什么。

---

## 1. 一句话定位

Zentin Payment 是面向 **ISV / 代理商 / 商户** 三级渠道体系的聚合支付平台：商户通过统一 API 接入，平台按路由与费率选择上游支付渠道完成收款、退款、转账与分账，并向商户回写通知。

---

## 2. 角色与租户层级

```
平台运营 (Platform)
├─ ISV（服务商）                        IsvNo
│  ├─ 代理商（一级/二级）               AgentNo
│  │  └─ 商户                           MchNo
│  │     ├─ 应用                        AppId  ── 对外调用凭据
│  │     ├─ 门店                        StoreId
│  │     ├─ 码牌 / 二维码               QrcId
│  │     └─ 会员                        MbrId  ── 商户侧储值与充值
│  └─ 商户（直营）                      MchNo
└─ 上游渠道                            IfCode
```

关键点：

- **租户维度是三条正交的号**：`IsvNo`、`AgentNo`、`MchNo`。订单表三个字段全部落库，用于对账、分润和权限过滤。
- **`AppId` 才是调用凭据的主体**，`MchNo` 是计费与结算主体。一个商户可以有多个应用。
- **会员（Mbr）不属于平台账户体系**，是商户自己的用户，用于储值/充值场景。平台账户流水是 `AccountBill`，会员流水是 `MbrBill`，两者不可混用。

| 角色 | 能做什么 | 对应宿主 |
| --- | --- | --- |
| 平台运营 | 全量配置、渠道接入、对账、风控、费率模板 | `Manager.Api` |
| ISV | 管理名下代理商与商户、配置渠道参数、查看分润 | `Manager.Api`（按权限收敛） |
| 代理商 | 管理名下商户、查看分润、配置商户费率 | `Agent.Api` |
| 商户 | 创建订单/退款/转账、查询、配置回调、门店与码牌 | `Merchant.Api` |
| 商户后端（机调） | 下单、查单、退款、转账、分账 | `Payment.Api` |

> 四个宿主共享同一 Application 层，差异只在认证方式、权限码与暴露的端点集合。

---

## 3. 业务域划分

按"变更原因是否相同"划界，不按数据表划界。

| 域 | 职责 | 核心聚合 | 是否本期 |
| --- | --- | --- | --- |
| **支付** | 下单、查单、关单、渠道回调、通知重发 | `PayOrder`、`MchNotifyRecord` | 是 |
| **退款** | 全额/部分退款、退款回调、退款重试 | `RefundOrder` | 是 |
| **转账/代付** | 单笔转账、转账回调、重试 | `TransferOrder` | 是 |
| **分账** | 分账接收方管理、分账执行、分账记录、利润 | `MchDivisionReceiver(Group)`、`PayOrderDivisionRecord`、`PayOrderProfit` | 是 |
| **对账** | 渠道账单拉取、批次、差异单 | `CheckBillBatch`、`ChannelBill`、`DiffBill` | 是（后期） |
| **资金账务** | 平台账户流水、会员账单与充值 | `AccountBill`、`MbrBill`、`MbrRechargeOrder`、`MbrRechargeRule` | 是（部分） |
| **接入配置** | 渠道定义、商户渠道参数、支付方式、费率、路由 | `PayInterfaceDefine`、`PayInterfaceConfig`、`MchPayPassage`、`PayWay`、`PayRateConfig` | 是 |
| **商户与渠道关系** | 商户、应用、门店、ISV、代理商、申请单 | `MchInfo`、`MchApp`、`MchStore`、`MchApply`、`IsvInfo`、`AgentInfo` | 是 |
| **收单工具** | 二维码、码牌 | `QrCode`、`QrCodeShell` | 是 |
| **设备** | 设备定义、渠道配置、设备实例 | `DeviceProviderDefine/Config`、`DeviceInfo` | 否（延后） |
| **系统管理** | 用户、角色、权限、配置、日志、文章 | — | **否，交给 IdentityServiceCenter** |

---

## 4. 核心业务流程

### 4.1 支付主链路（最重要，第一期只做这条）

```
商户后端
  │ POST /api/pay/create   (AppId + 签名 + MchOrderNo + Amount + WayCode)
  ▼
Payment.Api ── 验签/幂等/限流/权限
  ▼
PayOrder Application Handler
  ├─ 校验商户/应用/门店状态
  ├─ 路由：WayCode → MchPayPassage → PayInterfaceConfig → IfCode
  ├─ 计算费率与手续费
  ├─ 创建 PayOrder(State=Created)          ← 事务边界
  ├─ 调渠道 SPI: IPaymentService.Pay(...)
  ├─ 回写 ChannelOrderNo / 支付参数
  └─ 发布 PayOrderCreatedDomainEvent
  ▼
返回：渠道支付参数（二维码串 / 跳转 URL / JSAPI 参数）
```

异步回写：

```
上游渠道 ── 回调 ──▶ Payment.Api /api/channel/notice/{ifCode}
  ▼
IChannelNoticeService.Parse/Verify  →  ChannelNoticeResult
  ▼
PayOrder.MarkSuccess(...)              ← 状态机 + 幂等（按 ChannelOrderNo）
  ▼
Outbox 落库（同一事务）
  ▼
后台投递 → 商户回调 URL（MchNotifyRecord，指数退避重试）
```

### 4.2 退款链路

```
商户 POST /api/refund  → RefundOrder(Created) → IRefundService.Refund
上游回调 → RefundOrder.Success/Failed → PayOrder.RecordRefund → 商户通知
```

不变量：

- 累计退款额 ≤ 支付金额，且 ≤ 可退余额。
- 同一 `(MchNo, MchRefundNo)` 幂等。
- 退款成功必须回写 `PayOrder.RefundState` / `RefundAmount` / `RefundTimes`。

### 4.3 转账链路

`TransferOrder` 独立于支付订单，无 `PayOrderId` 关联。状态：创建 → 处理中 → 成功/失败。不能对已成功订单重复转账，按 `(MchNo, MchOrderNo)` 幂等。

### 4.4 分账链路

在支付成功后触发，两种模式：

- **同步分账**：支付回调里直接分。
- **异步分账**：定时/手动触发的分账记录重试。

不变量：`Σ 分账金额 ≤ 订单金额 - 手续费`；分账记录状态与渠道返回一致。

### 4.5 对账链路

按日拉取渠道账单 → 生成 `CheckBillBatch` → 落 `ChannelBill` → 与本地订单比对 → 生成 `DiffBill` → 人工处理。

---

## 5. 边界与非目标

明确不做，避免范围蔓延：

| 不做 | 原因 | 替代 |
| --- | --- | --- |
| 自建统一身份/登录 | 已有 IdentityServiceCenter（OpenIddict） | 复用 |
| 自建 API 网关 | 已有 ApiGateway（YARP） | 复用 |
| 自建调度中心 | 已有 SchedulerJob（Quartz） | 复用 |
| 自建消息队列 | 用 RabbitMQ + Outbox | 复用 |
| 前端控制台 | 不在本仓库 | 供体 ant-design-vue 仅参考 |
| 清算/牌照/备付金 | 非技术范围 | — |
| 实时风控引擎 | 超出练手范围 | 预留 `PayInterfaceConfig` 扩展位 |
| 多币种汇率 | 当前只有 `CNY` 语义 | 字段保留 `Currency` |

---

## 6. 术语表

| 术语 | 含义 | 备注 |
| --- | --- | --- |
| `IfCode` | 渠道代码，如 `wxpay` / `alipay` | 渠道 SPI 的注册键 |
| `WayCode` | 支付方式代码，如 `WX_JSAPI` / `ALI_QR` | 与 `IfCode` 是多对一 |
| `WayType` | 支付方式大类，如 `WXPAY` / `ALIPAY` | 用于路由与统计 |
| `MchNo` / `AppId` | 商户号 / 应用号 | 计费主体 / 调用主体 |
| `MchOrderNo` / `PayOrderId` | 商户单号 / 平台单号 | 幂等键 / 业务主键 |
| `ChannelOrderNo` | 上游渠道单号 | 回调对账关键字段 |
| 分账 | 把一笔收款按比例/固定额分给多个接收方 | 涉及 `MchDivisionReceiver` |
| 码牌 | 打印出来的固定收款二维码载体 | `QrCodeShell` + `QrCode` |
| 供体 | 被移植的 agpayplus 仓库 | 只读 |

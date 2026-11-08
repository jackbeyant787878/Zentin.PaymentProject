# ThirdPartyAdapters — 参考语料，不参与解决方案构建

本目录是从 agpayplus（Components.Third）移植的渠道适配层语料（15 渠道、354 个 .cs），
**已从 Zentin.Payment.slnx 移除**，原因：

- 它依赖 agpayplus 的应用服务面（`IPayOrderService`、`ISysConfigService`、`IOssService` 等），
  这些类型在 Zentin 解决方案中不存在，也不应该存在——目标架构中渠道适配器不得反向调用应用服务；
- 目标架构（WS8/R801）要求：SPI 七接口定义在 Domain/Application.Contracts，
  适配器作为 Infrastructure 侧的 SPI 实现，由应用层编排，配置经加密存储注入。

## 处置规则

1. **禁止就地修复**本目录编译错误（修复等于把旧耦合搬进新架构）；
2. 微信 / 支付宝适配器按 `new-channel-adapter` skill 从本语料**重新移植**到 SPI（Sprint S2/S3 卡片 R801-R806）；
3. 其余 13 渠道语料保留作报文规则字典，移植某渠道时再提取该渠道目录；
4. 移植提取报文 fixture 前必须脱敏（密钥、证书、卡号、签名体仅留摘要）。

重新纳入构建的唯一方式：完成 SPI 重移植后，以新工程替换本目录并加入 slnx。

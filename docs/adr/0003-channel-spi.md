# ADR-0003 · 渠道适配器独立项目 + 显式注册

- 状态：已采纳
- 日期：2026-11-08

## 背景

15 个上游渠道的签名、验签、报文拼装差异极大，但结构同构。供体把它们放在一个 `Components.Third` 项目里，通过 `Assembly.GetTypes()` + `Activator.CreateInstance` + `GetIfCode()` 反射推断注册键，并使用 `BuildServiceProvider()` 建第二次容器 + 静态 `ServiceResolver`。

问题：

- 注册键是运行期推断的，编译期无法校验；漏注册到运行期才暴露，且工厂吞掉异常返回 `default`。
- 二次容器造成单例/作用域泄漏。
- 适配器反向依赖应用服务（`IPayOrderService`、`ISysConfigService`、`IOssService`），形成环。

## 决策

1. **每个渠道一个独立项目**：`Zentin.Payment.Infrastructure.Channel.<IfCode>`，只引用 `Zentin.Payment.Application.Contracts` 和渠道 SDK。不得引用 `Application` 或 `Infrastructure`。
2. **SPI 接口放在 `Application.Contracts/Channel/`**：核心 7 个（下单/退款/查单/关单/转账/支付回调/退款回调）+ 扩展 4 个（转账回调/分账/分账回调/渠道用户）。
3. **显式注册**：每渠道一个 `Add<IfCode>Channel(this IServiceCollection)` 扩展，内部用 `AddKeyedScoped<T>(IfCode)`。`AddChannels()` 里逐行调用。
4. **工厂直连 DI**：`ChannelServiceFactory<T>.Get(ifCode)` 调 `GetRequiredKeyedService<T>`，找不到即抛异常，不吞。
5. **启动期自检**：注册的 `(接口, ifCode)` 集合必须覆盖配置表中启用的渠道；缺失则启动失败。
6. **保留报文逻辑原样**：只反转依赖方向，不改签名/验签/字段映射。

## 后果

好处：

- 注册键是编译期常量，漏注册在启动时即失败，而不是线上某渠道静默不可用。
- 每渠道项目可独立测试、独立引用 SDK，避免 SDK 版本冲突扩散。
- 适配器不依赖应用服务，依赖图无环。

代价与约束：

- 项目数增加（最多 15 个）。可用 solution folder 组织，不影响构建。
- 新增渠道必须同时改 `AddChannels()`，这是有意为之的显式成本。
- 供体语料不能直接编译，必须逐渠道提取；初期会感觉"重复劳动"，这正是练手要的吞吐训练。

## 替代方案

- **单项目 + 反射注册**：即供体做法，注册不可校验、易漏、难排查，否决。
- **单项目 + 显式注册**：可行，但 SDK 依赖会集中在同一项目，版本冲突风险高，且无法按渠道独立测试。若后期项目数造成负担，可退回此方案。

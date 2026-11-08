# AGENTS.md — Zentin Payment

本文件是 AI 代理在本仓库的**常驻契约**，每次会话开始前必须先读。与任何其他文档冲突时，以本文件为准；需要改规则时先改本文件，再改代码。

---

## 1. 项目定位

Zentin Payment 是三级租户（ISV / 代理商 / 商户）的聚合支付平台，从 agpayplus 语义移植并重构为 .NET 10 模块化单体。

| 项 | 值 |
| --- | --- |
| 唯一工作副本 | `D:\Projects\Zentin.Payment` |
| 解决方案 | `Zentin.Payment.slnx` |
| 供体仓库（只读） | `D:\Projects\agpayplus` |
| 供体锁定 commit | `ba61fea9cbbcd1c6940e5b3ef8b55284e2ab3eac` |
| 目标框架 | .NET 10（SDK 10.0.403） |
| 数据库 | SQL Server + EF Core 10 Code First |
| 前端 | 不在本仓库范围 |

供体仓库**不构建、不修改、不同步**。它只作为语义参考和渠道报文规则字典。移植是"器官移植"，不是搬家。

---

## 2. 铁律

违反以下任一条即为不合格变更，不看其他优点。

1. **依赖只能向内**：`Api → Application → Domain ← Infrastructure`。
   - Domain 不得引用 EF Core、ASP.NET Core、MediatR、AutoMapper、Newtonsoft.Json 或任何 `Microsoft.*` 包。
   - Domain 不得引用 `Application` / `Application.Contracts` / `Infrastructure`。
   - 领域命令与 DTO 归属 Application，不放在 Domain。
2. **`IQueryable` 不得离开 Application**。仓储只返回实体、`IReadOnlyList<T>`、分页结果或标量。
3. **金额一律 `long`，单位：分**。费率一律 `decimal`，精度 `HasPrecision(20, 6)`。禁止 `double` / `float` 参与任何金额计算。
4. **渠道适配器是防腐层（ACL）**：保留渠道签名、报文拼装、验签逻辑原样，只反转依赖方向。禁止对适配器做"顺手 DDD 化"。
5. **禁止反射扫描注册渠道服务**。不得用 `Assembly.GetTypes()` + `Activator.CreateInstance` + `GetIfCode()` 推断注册键；使用显式注册表。
6. **禁止 `services.BuildServiceProvider()`** 构造第二个容器；禁止静态 `ServiceResolver`。
7. **跨模块只能通过 Application 端口或领域事件**。不得直接引用其他模块的 DbContext 或实体。
8. **一次只做一个垂直切片**。不做未被要求的重构、不改无关文件、不加"以后可能用到"的抽象。
9. **声称完成必须给证据**：贴出执行的命令和输出。`dotnet build` 通过 ≠ 系统可运行。
10. **不猜语义**。供体行为不明确时，先读供体源码并给出 `文件:行号`，或提问；不得自行编造业务规则。

---

## 3. 目录与命名

```
Zentin.Payment/
├─ AGENTS.md                       本契约
├─ docs/                           计划与架构文档
│  ├─ 00-index.md
│  ├─ 01-product-architecture.md
│  ├─ 02-technical-architecture.md
│  ├─ 03-module-inventory.md
│  ├─ 04-data-model.md
│  ├─ 05-migration-plan.md
│  ├─ 06-ai-coding-playbook.md
│  └─ adr/                         架构决策记录
├─ src/                            目标：所有生产代码迁入 src/
│  ├─ Zentin.Payment.Domain/
│  ├─ Zentin.Payment.Application.Contracts/
│  ├─ Zentin.Payment.Application/
│  ├─ Zentin.Payment.Infrastructure/
│  ├─ Zentin.Payment.Infrastructure.Channel.<IfCode>/   渠道适配器（每渠道独立）
│  ├─ Zentin.Payment.Api/                               支付主链 API
│  ├─ Zentin.Payment.Manager.Api/
│  ├─ Zentin.Payment.Merchant.Api/
│  └─ Zentin.Payment.Agent.Api/
├─ tests/
└─ reference/                      参考语料，不参与构建
```

命名规则：

| 类型 | 规则 | 示例 |
| --- | --- | --- |
| 解决方案/项目 | `Zentin.Payment.<Layer>` | `Zentin.Payment.Domain` |
| 聚合根 | 名词，`sealed`，私有 setter，私有构造 + 静态工厂 | `PayOrder.Create(...)` |
| 领域事件 | `<聚合><过去时>DomainEvent` | `PayOrderPaidDomainEvent` |
| 仓储端口 | `I<聚合>Repository`，文件同名带 `I` | `IPayOrderRepository.cs` |
| EF 配置 | `<聚合>EntityTypeConfiguration` | `PayOrderEntityTypeConfiguration.cs` |
| 命令/查询 | `<动作><聚合>Command` / `<聚合><条件>Query` | `CreatePayOrderCommand` |
| 表名 | PascalCase 单数 | `PayOrder` |
| 主键 | 业务字符串主键，`ValueGeneratedNever()` | `PayOrderId` |
| 文件名 | 禁止首尾空格（Windows/git 会出问题） | — |

---

## 4. 验证命令

在仓库根目录执行。按顺序，前一步失败不要往下走。

```bash
# 1) 编译
dotnet build Zentin.Payment.slnx --nologo

# 2) EF 模型守卫（必须绿；红的含义是迁移生成不了、运行时 DbContext 不可用）
dotnet test Zentin.Payment.Domain.Tests/Zentin.Payment.Infrastructure.Tests.csproj --nologo -v q

# 3) 领域单测
dotnet test Zentin.Payment.Domain.Tests --nologo -v q

# 4) 全量测试
dotnet test Zentin.Payment.slnx --nologo -v q
```

**判断标准**：`0 errors` 只是最低门槛。必须同时满足 EF 模型守卫测试全绿，才算"能跑"。

新增迁移：

```bash
dotnet ef migrations add <Name> \
  --project Zentin.Payment.Infrastructure \
  --startup-project Zentin.Payment.Api \
  --output-dir Migrations
```

---

## 5. 完成定义（DoD）

一个切片只有全部满足才算完成，缺一项即"未完成"：

- [ ] Domain 聚合不变量、状态迁移、领域事件有单测覆盖
- [ ] 仓储端口只暴露实体/只读集合，无 `IQueryable`
- [ ] EF 配置 + DbSet + 迁移齐备，EF 模型守卫测试绿
- [ ] Command/Query + Handler + Validator 齐备
- [ ] API 端点带显式权限码
- [ ] 至少 1 个集成测试（真实 SQL Server 或 Testcontainers）
- [ ] `docs/03-module-inventory.md` 中该切片状态已更新
- [ ] 与供体的行为差异写入 `legacy-parity` 记录（含 `文件:行号` 证据）
- [ ] 有架构争议的决策已写 ADR
- [ ] 记录本轮 AI 协作度量（首次通过率 / 返工轮数 / 人工介入次数）

---

## 6. 已知技术债（已确认，不要自行"发现"后顺手大改）

截至 2026-11-08 核实的现状：

| 编号 | 问题 | 证据 | 归属阶段 |
| --- | --- | --- | --- |
| TD-01 | EF 模型建不起来：`HasConversion<byte>()` 后 `HasDefaultValue((byte)...)` 类型不匹配 | `Pay/RefundOrderEntityTypeConfiguration.cs:20`（已合并修复） | P0 |
| TD-02 | `bool NotifyState` 配 `HasDefaultValue(0)` | `Order/PayOrderEntityTypeConfiguration.cs:42`（已修复） | P0 |
| TD-03 | `MbrRechargeRule` 两份实体都 `ToTable("MbrRechargeRule")`，两实体撞同一表名 | `Entities/Member/` 与 `Entities/Merchant/` | P0 |
| TD-04 | 全量重建约 2600 条警告，CS8618 占 2170（83%），淹没真信号 | `dotnet build --no-incremental -v n` | P0 |
| TD-05 | Domain 引用 Application.Contracts 且内含 EF Core/AutoMapper/MediatR | `Zentin.Payment.Domain.csproj` | P1 |
| TD-06 | 领域实体 `TransferOrder` 曾声明在 `Infrastructure.EntityTypeConfiguration.Pay` 命名空间 | 已修正 | P0 |
| TD-07 | `DomainNotification` 曾声明在 `Application.Contracts.Notifications` | 已修正 | P0 |
| TD-08 | DbContext 仅 20 个显式 DbSet，另有 12 个实体只有配置无 DbSet | `PaymentDbContext.cs` | P1 |
| TD-09 | "更完整的聚合版本反而是死代码"（PayOrder / RefundOrder / MbrRechargeRule 三例） | 见 `docs/04-data-model.md` | P0-P1 |
| TD-10 | `Zentin.Payment.ThirdPartyAdapters` 已从 slnx 移除，仅作语料 | 该目录 `README.md` | 长期 |
| TD-11 | 四个 API 宿主仍是模板，无认证、无统一响应、无异常中间件 | `Zentin.Payment.Api/Program.cs` | P5 |

处理顺序见 `docs/05-migration-plan.md`。**不要**在做一个切片的途中顺手清理上表其他条目，除非该条目是当前切片的阻塞项。

---

## 7. 文档地图

| 想知道 | 读 |
| --- | --- |
| 明天干什么 | `docs/00-index.md` 第 4 节 |
| 产品要做什么、不做什么 | `docs/01-product-architecture.md` |
| 分层、模块、渠道 SPI、运行时 | `docs/02-technical-architecture.md` |
| 有哪些模块/切片/渠道，各自进度 | `docs/03-module-inventory.md` |
| 表结构、状态机、索引 | `docs/04-data-model.md` |
| 排期、每阶段 DoD、切片模板 | `docs/05-migration-plan.md` |
| AGENTS.md/skills/MCP/度量怎么写 | `docs/06-ai-coding-playbook.md` |
| 某个决策为什么这么定 | `docs/adr/` |

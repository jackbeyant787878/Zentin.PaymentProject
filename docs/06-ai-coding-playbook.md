# 06 · AI Coding 实战手册

> 目标不是"让 AI 写更多代码"，而是**让每一次 AI 产出都能被快速证伪**。反馈环越密，学得越快。

---

## 1. 核心论点

学习效率 ≈ 反馈环密度 × 任务同构度 ÷ 上下文重建成本。

三个变量在这个项目里怎么优化：

| 变量 | 做法 |
| --- | --- |
| 反馈环密度 | EF 模型守卫、领域单测、fixture 测试、迁移 diff——每个都能在秒级证伪 |
| 任务同构度 | 22 个切片用同一模板；15 个渠道用同一模板 |
| 上下文重建成本 | `AGENTS.md` + `docs/` + ADR 三层持久化，冷启动不用重新解释 |

---

## 2. AGENTS.md 怎么写

### 2.1 它是什么

`AGENTS.md` 是**每次会话都加载的契约**，不是教程，不是架构文档。判断标准：如果一条内容只在做某类任务时才需要，它属于 skill，不属于 `AGENTS.md`。

### 2.2 该写什么

- 项目一句话定位与唯一路径
- 依赖方向与分层铁律（可被单条违反与否判定）
- 目录与命名约定
- 验证命令（复制即可执行）
- DoD 清单
- **已知技术债清单**（防止 AI "发现"后自作主张大改）
- 文档地图

### 2.3 不该写什么

| 不写 | 原因 |
| --- | --- |
| 大段代码示例 | 挤占上下文，且会过期 |
| 完整表结构 | 属于 `04-data-model.md`，按需读 |
| 排期 | 属于 `05-migration-plan.md` |
| "请写出高质量代码" | 无法判定，等于没写 |
| 业务规则细节 | 属于切片契约文档，逐切片加载 |

### 2.4 长度

控制在 150 行以内。超过就说明有内容该下沉到 `docs/` 或 skill。

### 2.5 维护触发条件

出现以下任一情况，当天就改 `AGENTS.md`：

1. AI 犯了同一个错第二次
2. 你手动纠正了一个 AI 本不该犯的错
3. 某条规则需要解释才能执行（说明写得不够硬）
4. 技术债清单发生变化

---

## 3. Skills 怎么写

### 3.1 什么是 skill

按需加载的操作规程。一个 skill 对应一类重复任务，包含：触发条件、前置检查、步骤、验收、常见失败模式。

### 3.2 清单

| Skill | 触发 | 产出 |
| --- | --- | --- |
| `add-vertical-slice` | 要新增一个业务切片 | Domain→Api 全链路 + 测试 |
| `ef-migration` | 要改表结构 | EF 配置 + 迁移 + 守卫测试通过 |
| `port-channel` | 要移植一个渠道 | 独立渠道项目 + 6 个动作 + fixture |
| `legacy-parity` | 要与供体行为对照 | 行为差异表（带 `文件:行号`） |
| `code-review-gate` | 提交前 | 铁律逐条检查结果 |
| `write-adr` | 出现架构争议 | 一份 ADR |

### 3.3 目录结构

```
.qoder/skills/            （或你所用工具的 skills 目录）
├─ add-vertical-slice/
│  └─ SKILL.md
├─ ef-migration/
│  └─ SKILL.md
├─ port-channel/
│  └─ SKILL.md
├─ legacy-parity/
│  └─ SKILL.md
└─ code-review-gate/
   └─ SKILL.md
```

### 3.4 完整示例：`add-vertical-slice/SKILL.md`

```markdown
---
name: add-vertical-slice
description: 在 Zentin Payment 中新增一个垂直业务切片（Domain 到 Api 全链路）。当用户要求"实现 X 切片/用例/接口"时使用。
---

# 新增垂直切片

## 前置检查
1. 读 `AGENTS.md` 第 2 节铁律与第 5 节 DoD。
2. 确认 `docs/slices/<slice>.md` 契约文档已存在；不存在则先创建，不要直接写代码。
3. 确认 `dotnet build` 与 EF 模型守卫当前是绿的。不绿先修基线。

## 步骤
1. Domain：聚合（sealed、私有 setter、私有构造 + 静态工厂）、值对象、枚举、领域事件、领域异常。
2. Domain：不变量与状态迁移单测，先跑红。
3. Domain：`I<X>Repository` 端口，禁止返回 `IQueryable`。
4. Infrastructure：`<X>EntityTypeConfiguration`（一个实体只能有一份）+ DbSet。
5. Infrastructure：`dotnet ef migrations add Add<Slice>`，检查生成的 Up()。
6. Application.Contracts：Command/Query + DTO + 结果契约。
7. Application：Handler + Validator（用 FluentValidation 或平台等价物）。
8. Api：端点 + 权限码 + 统一响应。
9. 测试：仓储集成测试 + Handler 测试。
10. 更新 `docs/03-module-inventory.md` 该切片状态。

## 验收
- `dotnet build Zentin.Payment.slnx --nologo` 0 errors
- `dotnet test Zentin.Payment.Domain.Tests/Zentin.Payment.Infrastructure.Tests.csproj` 全绿
- 新代码零 CS8618
- 不变量与非法状态迁移都有测试
- 没有新增警告类型

## 常见失败模式
- 把 Handler 写成事务脚本，业务规则留在 Application → 规则必须下沉到聚合。
- 仓储返回 `IQueryable` 让 API 组装查询 → 禁止。
- 顺手改了无关文件 → 回退，记入待办。
- 迁移里出现非预期的表/列 → 检查是否 `ApplyConfigurationsFromAssembly` 把无 DbSet 的实体也带进来了。
```

### 3.5 完整示例：`ef-migration/SKILL.md`

```markdown
---
name: ef-migration
description: 为 Zentin Payment 增改 EF Core 配置并生成迁移。当需要新增表、字段、索引或修复模型错误时使用。
---

# EF 配置与迁移

## 前置检查
1. `dotnet test ...Infrastructure.Tests` 必须绿；红了先修模型。
2. 确认该实体只有一份 `IEntityTypeConfiguration`。
3. 确认目标表名没有被其他实体类型占用。

## 步骤
1. 写/改 `<X>EntityTypeConfiguration`。
2. 枚举默认值必须写 `HasDefaultValue(EnumMember)`，不能写 `0`。
3. `bool` 默认值写 `HasDefaultValue(false)`。
4. 金额 `bigint`，费率 `decimal(20,6)`，时间 `datetime2(6)`。
5. 生成迁移：
   `dotnet ef migrations add <Name> --project Zentin.Payment.Infrastructure --startup-project Zentin.Payment.Api --output-dir Migrations`
6. 打开生成的迁移，逐行检查 `Up()`：表、列、索引、FK 是否与预期一致。
7. 跑守卫测试 + `database update`（本地库）。

## 验收
- 守卫测试 2/2 绿
- 迁移 `Up()` 与 `Down()` 对称
- 没有表名冲突
- 没有把无 DbSet 的实体意外带入

## 常见失败模式
- `HasDefaultValue(0)` 用在枚举/bool 上 → 模型构建抛 InvalidOperationException。
- 两份配置指向同一实体 → 后者静默覆盖前者，FK 名/索引不可复现。
- 两个实体映射同一表名 → 模型验证失败。
```

---

## 4. MCP 配置

### 4.1 原则

- **只读**。数据库 MCP 一律用只读连接，且用只有 `db_datareader` 权限的账号。
- **不连接生产库**。只连本地开发库。
- **不把 MCP 当捷径**。能用代码回答的问题不要查库。

### 4.2 需要哪些

| MCP | 用途 | 必要性 |
| --- | --- | --- |
| SQL Server（只读） | 核对迁移结果、验证数据形态、排查 EF 生成的 SQL | 推荐 |
| 文件系统 | 本项目已有原生 Read/Grep/Glob，一般不需要 | 可选 |
| MySQL（只读） | 仅当本地有供体 agpayplus 的数据集时，用于对照字段语义 | 可选 |

### 4.3 配置要点（任何客户端通用）

```json
{
  "mcpServers": {
    "zentin-sqlserver-ro": {
      "command": "<你选用的 SQL Server MCP 可执行文件>",
      "args": ["--connection", "Server=localhost;Database=ZentinPayment;User Id=zentin_ro;Password=***;TrustServerCertificate=true"],
      "env": {}
    }
  }
}
```

配套 SQL：

```sql
CREATE LOGIN zentin_ro WITH PASSWORD = '<强密码>';
CREATE USER zentin_ro FOR LOGIN zentin_ro;
ALTER ROLE db_datareader ADD MEMBER zentin_ro;
-- 不给 db_datawriter / db_ddladmin
```

### 4.4 用 MCP 能回答的问题

- 迁移后表结构与预期是否一致
- 某个索引是否真的建了
- EF 生成的 SQL 是否出现全表扫描
- 种子数据是否符合预期

**不要**用 MCP 回答"业务规则应该是什么"——那必须读供体代码。

---

## 5. 三层记忆

| 层 | 回答 | 存哪 | 更新时机 |
| --- | --- | --- | --- |
| **ADR** | 为什么这么决定 | `docs/adr/` | 出现架构争议、且有两个以上合理选项时 |
| **项目记忆** | 现在是什么状态 | 工具的 memory 目录 | 状态变化时（非代码可推导的事实） |
| **文档** | 系统是什么 | `docs/` | 每个切片/阶段结束 |

### 5.1 什么不该进记忆

- 代码里能读到的（分层、命名、表结构）
- git 历史能查到的（谁改了什么）
- 一次性的调试过程
- `AGENTS.md` 已写的规则

### 5.2 什么该进 ADR

| 该写 | 不该写 |
| --- | --- |
| 模块化单体 vs 微服务 | "今天修了个 bug" |
| 金额用 long 分 vs decimal | "改了三个文件" |
| 渠道适配器独立项目 vs 单项目 | 排期 |
| 显式注册 vs 反射注册 | 教程 |

### 5.3 ADR 模板

```markdown
# ADR-NNNN · 标题

- 状态：提议 / 已采纳 / 已废弃
- 日期：YYYY-MM-DD

## 背景
什么约束逼出了这个决定。

## 决策
一句话说清选了什么。

## 后果
好处、代价、需要遵守的新规则。

## 替代方案
还考虑过什么，为什么不选。
```

---

## 6. 提示词范式

五句可复用的话，比长篇需求描述有效。

### 6.1 划定范围

> 只修改与 `<切片>` 相关的文件。发现其他问题写进 `docs/03-module-inventory.md` 的待办，不要当场改。

### 6.2 要求证据

> 每条业务规则必须给出供体 `文件:行号`。没有证据的地方标注"待确认"，不要自行推断。

### 6.3 先契约后实现

> 先输出命令/查询契约、状态迁移、幂等键、表字段映射。我确认后再写代码。

### 6.4 要求可验证

> 完成后贴出你实际执行的命令与输出。`dotnet build` 通过不算完成，EF 模型守卫必须绿。

### 6.5 强制红-绿

> 先写会失败的测试并贴出失败输出，再写实现让它变绿。不允许先实现再补测试。

### 6.6 反对"聪明"

> 不要引入我没要求的抽象、基类、扩展方法或配置项。三个相似的行优于一个过早的抽象。

---

## 7. 红-绿-重构工作流

```
1. 写测试（必须失败）
   └─ 贴出失败输出，证明测试真的在测东西
2. 写最小实现
   └─ 只让当前测试通过，不多写
3. 跑全量
   └─ build + EF 守卫 + 领域测试 + 集成测试
4. 重构（可选）
   └─ 测试必须保持绿
5. 写回
   └─ 更新清单/ADR/度量
```

为什么强调"测试必须先失败"：本项目已经出现过一次"构建全绿但 EF 模型根本建不起来"的情况。一个从未红过的测试，不能证明它能发现任何问题。

### 7.1 每日命令

```bash
# 快速反馈（秒级）
dotnet build Zentin.Payment.slnx --nologo
dotnet test Zentin.Payment.Domain.Tests/Zentin.Payment.Infrastructure.Tests.csproj --nologo -v q

# 提交前（分钟级）
dotnet test Zentin.Payment.slnx --nologo -v q
git diff --stat
```

---

## 8. 反模式（都真实发生过）

### 8.1 "构建绿就宣布完成"

**事实**：2026-11-08 实测 `0 errors`，但 EF 模型因 `HasDefaultValue((byte)...)` 类型错误无法构建，迁移生成不了，DbContext 运行期不可用。

**对策**：完成标准必须包含 EF 模型守卫与至少一个集成测试。

### 8.2 警告噪音淹没信号

**事实**：全量重建约 2600 条警告，CS8618 占 2170 条（83%），真实错误被埋在中间，排查时必须反复翻页。

**对策**：先建立警告基线（P0-6），再开始写业务代码。

### 8.3 更完整的代码反而是死代码

**事实**：`PayOrder` 有两份，`Pay/` 版本有 `MarkPaying`/`MarkFail`/`RecordRefund` 与 `IReadOnlyCollection` 封装，但 DbContext 用的是行为更少的 `Order/` 版本。`RefundOrder`、`MbrRechargeRule` 同构。

**对策**：改任何实体前先确认"哪一份被映射"。`AGENTS.md` 第 6 节列出重复项。

### 8.4 反射注册渠道服务

**事实**：供体用 `Assembly.GetTypes()` + `Activator.CreateInstance` + `GetIfCode()` 推断注册键，还 `BuildServiceProvider()` 建二次容器。

**对策**：显式注册表，编译期常量键，启动期自检。见 `02` 第 4.2 节。

### 8.5 "顺手把适配器 DDD 化"

**事实**：渠道适配层依赖应用服务（`IPayOrderService`、`ISysConfigService`、`IOssService`），有人会想直接改成依赖注入的领域接口。

**对策**：只反转依赖，不动签名与报文逻辑。适配器只引用 `Application.Contracts`。

### 8.6 AI 顺手重构无关文件

**对策**：提示词 6.1 明确划范围；提交前 `git diff --stat` 检查文件数。

### 8.7 AI 编造业务规则

**对策**：提示词 6.2 要求 `文件:行号`；切片契约里设"待确认"栏。

---

## 9. 每周复盘流程（30 分钟）

1. 填 `05-migration-plan.md` 第 7 节的度量表。
2. 回答三个问题：
   - 哪个错误 AI 犯了两次？→ 改 `AGENTS.md` 或 skill
   - 哪个任务返工最多？→ 说明契约没写清，补充 `docs/slices/`
   - 哪个测试没红过就绿了？→ 检查它是否真的有效
3. 更新 `03-module-inventory.md` 进度。
4. 下周最多定 3 个目标。

---

## 10. 一个可执行的最小会话模板

每次开始新会话，把下面这段贴给 AI：

```
读 AGENTS.md 和 docs/00-index.md。
本轮只做：<一个切片或一个 P0 项>。
先给我契约（命令/查询、状态迁移、表字段映射、供体证据），我确认后再写代码。
发现范围外的问题只记录，不要改。
完成后贴出 dotnet build 与 EF 模型守卫的实际输出。
```

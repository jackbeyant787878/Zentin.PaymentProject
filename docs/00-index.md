# 00 · 总索引与开工第一天

> 这份计划假定你在**全新的工作区**里执行。所有路径、命令、文件清单都以 `D:\Projects\Zentin.Payment` 为根。

---

## 1. 五分钟上手（新工作区）

```bash
# 1. 确认工作副本与基线
cd /d/Projects/Zentin.Payment
git log --oneline -1          # 应为 ba58635 或之后的提交
git status --short            # 确认没有别人的未提交改动

# 2. 还原依赖并编译
dotnet restore Zentin.Payment.slnx
dotnet build Zentin.Payment.slnx --nologo

# 3. 跑 EF 模型守卫（关键判据）
dotnet test Zentin.Payment.Domain.Tests/Zentin.Payment.Infrastructure.Tests.csproj --nologo -v q
```

**判读方式**：

- `dotnet build` 0 errors：只说明语法和类型没问题。
- EF 模型守卫**必须绿**。它红了就代表迁移生成不了、DbContext 运行期不可用。这是本项目当前的真实状态，见第 3 节。

---

## 2. 文档地图

| 文件 | 内容 | 什么时候读 |
| --- | --- | --- |
| [`../AGENTS.md`](../AGENTS.md) | AI 常驻契约、铁律、DoD | **每次会话开始** |
| [`01-product-architecture.md`](01-product-architecture.md) | 角色、业务域、主链路、非目标、术语 | 动手前读一次 |
| [`02-technical-architecture.md`](02-technical-architecture.md) | 分层、模块边界、渠道 SPI、状态机、横切 | 动手前读一次 |
| [`03-module-inventory.md`](03-module-inventory.md) | 22 个业务切片 + 15 个渠道 + 进度表 | 每周更新 |
| [`04-data-model.md`](04-data-model.md) | 表清单、核心表设计、状态机、索引、迁移策略 | 做持久化时读 |
| [`05-migration-plan.md`](05-migration-plan.md) | 阶段排期、每阶段 DoD、切片模板、风险 | 排期与验收时读 |
| [`06-ai-coding-playbook.md`](06-ai-coding-playbook.md) | AGENTS.md/skills/MCP/记忆/度量/反模式 | 想提升 AI 协作效率时读 |
| [`adr/`](adr/) | 架构决策记录 | 遇到"为什么这么定"时读 |

阅读顺序：`AGENTS.md` → `01` → `02` → `05` 的 P0 → 开工。

---

## 3. 当前基线（2026-11-08 核实）

| 项 | 实测 | 含义 |
| --- | --- | --- |
| 构建 | `0 errors`，全量重建约 2600 warnings | 能编译，但警告淹没信号 |
| 警告构成 | CS8618: 2170（83%）、CS8603: 162、CS8625: 84、CS8600: 66、CS8604/8602/0168: 各 34、SYSLIB0057/1224: 6 | 96% 是 nullable 噪音 |
| EF 模型 | **不可构建**（守卫测试 2/2 失败） | 迁移生成不了 |
| 显式 DbSet | 20 个 | 另有 12 个实体只有配置无 DbSet |
| 实体文件 | 37 个 → 去重后 34 个 | 供体 agpayplus 为 33 个，非一一对应 |
| 仓储 | 22 个接口 + 22 个实现 | 缺 Mch/Agent/Isv/Member 等 |
| 业务切片 | 0 个端到端打通 | DTO 齐、领域模型散、无 Handler |
| 渠道适配器 | 15 个目录，354 个 .cs，已移出 slnx | 仅作语料 |
| API 宿主 | 4 个，均为默认模板 | 无认证/统一响应/异常处理 |

**工作树中已有未提交的 P0 改动**（2026-11-08 会话产生）：

1. 4 个重命名：3 个尾空格文件名 + `TransferOrderRepository.cs` → `ITransferOrderRepository.cs`
2. `TransferOrder` 命名空间从 `Infrastructure.EntityTypeConfiguration.Pay` 修正为 `Domain.Entities.Pay`；`DomainNotification` 修正为 `Domain.Abstractions`
3. RefundOrder 两份 EF 配置合并为一份，修复 `HasDefaultValue` 类型错误
4. 删除 `Entities/Pay/RefundOrder.cs`、`Pay/RefundOrderEntityTypeConfiguration.cs`、`Domain.Tests/Test1.cs`
5. 新增 `Domain.Tests/PaymentDbContextModelTests.cs`（EF 模型守卫）+ 给该 csproj 加 Infrastructure 引用
6. `PayOrderEntityTypeConfiguration.cs` 的 `NotifyState` 默认值改为 `false`

开工第一步：`git diff` + `git diff --cached` review 这 6 项，确认无误后作为一个提交落地（建议提交信息：`chore(p0): 修复 EF 模型阻塞与实体重复`）。如果你不想保留，直接按 `05-migration-plan.md` 的 P0 清单重做。

---

## 4. 第一周逐日清单（2026-11-09 起）

每天目标 2~3 小时。**每天结束必须有一个可验证的产出**，不接受"今天读代码"。

### D1 · 11-09 周一：让 EF 模型建起来

- [ ] review 并提交工作树中的 P0 改动
- [ ] 跑守卫测试，读取下一个根因（测试已改成自动输出根因摘要的话更快；当前版本需看失败块开头）
- [ ] 逐个修复 `HasDefaultValue` / 表名冲突，直到守卫 2/2 绿
- [ ] 解决 `MbrRechargeRule` 双份：确定归属（建议 `Member`，因业务语义是会员充值规则），合并成员，删除另一份，同步删除其 EF 配置
- [ ] 产出：`dotnet test ...Infrastructure.Tests` 输出 `总计: 2 / 失败: 0`

**判据**：`context.Model` 能构建，且没有两张表同名。

### D2 · 11-10 周二：补全持久化覆盖面

- [ ] 列出 34 个实体中哪些有 DbSet、哪些只有配置、哪些都没有，写入 `03-module-inventory.md`
- [ ] 为 `MchInfo` / `MchApp` / `MchStore` / `MchApply` / `AgentInfo` / `IsvInfo` / `AccountBill` 补 DbSet（仅当对应 EF 配置已存在；没有配置的先不补）
- [ ] 生成第一个迁移 `InitialCreate`（如果仓库还没有 `Migrations/`）
- [ ] 产出：`Migrations/` 目录 + 迁移成功日志

**判据**：`dotnet ef migrations add` 成功，且生成的 `InitialCreate` 覆盖上述表。

### D3 · 11-11 周三：警告基线

- [ ] 新建根目录 `Directory.Build.props`：
  - `<Nullable>enable</Nullable>`、`<TreatWarningsAsErrors>false</TreatWarningsAsErrors>`
  - `<WarningsAsErrors>CS0108;CS0114;CS0162;CS0168;CS0219;CS0472;CS0618;SYSLIB0057;SYSLIB1224</WarningsAsErrors>`（未使用变量、过时 API 等真信号升级为错误）
  - `<NoWarn>CS8618</NoWarn>` **仅对 DTO/Commands 目录**（用 csproj 里的 `Compile Update` 条件设置，不要全项目关）
- [ ] 修 34 处 CS0168（`catch (Exception ex)` 未使用 → `catch (Exception)`）
- [ ] 替换 6 处 SYSLIB0057/1224 过时 API
- [ ] 产出：全量重建警告从 ~2600 降到 < 300，且新代码的 CS8618 不再出现

**判据**：`dotnet build --no-incremental` 警告数可在 PR 里一眼看完。

### D4 · 11-12 周四：搭 AI 协作底座

- [ ] 提交 `AGENTS.md` 与本 `docs/` 全套
- [ ] 创建 `.qoder/skills/`（或你所用工具的 skills 目录），先落 2 个：`add-vertical-slice`、`ef-migration`
- [ ] 写 `docs/adr/0001-modular-monolith.md`、`0002-money-and-precision.md`
- [ ] 产出：新开一次会话，验证 AI 能仅凭 `AGENTS.md` 正确说出"金额用什么类型、Domain 能引用什么"

**判据**：一次冷启动会话不犯分层错误。

### D5 · 11-13 周五：第一个垂直切片的设计（不写实现）

- [ ] 精读供体 `D:\Projects\agpayplus\aspnet-core\src\AGooday.AgPay.Application\Services\PayOrderService.cs`
- [ ] 精读 `AGooday.AgPay.Domain\Models\PayOrder.cs`
- [ ] 产出 `docs/slices/pay-order-create.md`：命令契约、校验规则、状态迁移、幂等键、领域事件、表字段映射、与供体的差异清单（每条带 `文件:行号`）
- [ ] 不做实现

**判据**：契约文档能让另一个人（或另一个 AI 会话）直接照着写实现。

### D6 · 11-14 周六：实现 PayOrder 下单切片

- [ ] 按 `05-migration-plan.md` 的 10 步切片模板执行
- [ ] 只做 `创建支付订单` 一条路径；不碰退款、不碰渠道调用
- [ ] 产出：Domain 单测 + Handler 单测 + 1 个集成测试全绿

**判据**：`POST /api/pay/create` 能写入 `PayOrder` 表并返回业务单号。

### D7 · 11-15 周日：复盘与写回

- [ ] 记录本周 AI 度量（首次通过率 / 返工轮数 / 人工介入次数）
- [ ] 把踩到的坑写回 `AGENTS.md` 第 6 节或对应 skill 的 checklist
- [ ] 更新 `03-module-inventory.md` 的进度列
- [ ] 规划下周：继续支付主链（查询 → 回调 → 关单）

**判据**：下周开工时，AI 冷启动不再犯本周犯过的错。

---

## 5. 怎么用这份计划跟 AI 协作

1. **一次一个切片**。把 `docs/slices/<slice>.md` 和 `AGENTS.md` 一起给 AI，不要口头描述需求。
2. **先要契约，再要实现**。让 AI 先产出命令/查询契约与状态迁移，你确认后再让它写代码。
3. **要求证据**。任何"已完成"必须附命令与输出；任何业务规则必须附供体 `文件:行号`。
4. **红-绿**。先让测试红（证明测试有效），再让实现变绿。禁止先写实现再补测试。
5. **禁止顺手重构**。把发现的问题记进 `03-module-inventory.md` 的"待办"，不要当场改。
6. **每轮结束写回**。踩坑写进 `AGENTS.md`/skill，决策写进 ADR，进度写进清单。否则下一轮 AI 会重犯。

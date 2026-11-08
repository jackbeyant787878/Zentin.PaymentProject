# ADR-0004 · EF 模型以测试为闸门，一实体一配置

- 状态：已采纳
- 日期：2026-11-08

## 背景

2026-11-08 实测：`dotnet build` 全绿（0 errors），但 EF 模型无法构建，报

```
Default value '0' of type 'byte' cannot be set on property 'State'
of type 'Enums+RefundOrderStatus' in entity type 'RefundOrder'.
```

原因有三类：

1. `HasConversion<byte>()` 之后 `HasDefaultValue((byte)X)`——默认值仍要求 CLR 类型。
2. `bool` 属性配 `HasDefaultValue(0)`。
3. 两个实体类型映射同一表名（`MbrRechargeRule`），以及同一实体有两份 `IEntityTypeConfiguration`（`RefundOrder`），后者静默覆盖前者，FK 约束名不可复现。

后果是迁移生成不了、DbContext 运行期不可用，而这一切在编译期完全不可见。

## 决策

1. **EF 模型守卫测试作为准入闸门**：`tests/Zentin.Payment.Infrastructure.Tests/PaymentDbContextModelTests.cs`
   - `Model_builds_without_validation_errors`
   - `Each_table_is_mapped_by_exactly_one_entity_type`
   两者必须绿才能提交。
2. **一个实体类型只能有一份 `IEntityTypeConfiguration`**，且类名与文件名为 `<Entity>EntityTypeConfiguration`。
3. **一个表名只能被一个实体类型映射**。
4. **默认值类型必须匹配属性 CLR 类型**：枚举写枚举值，`bool` 写 `false`。
5. **关系只在一处声明**：一对多关系在主体侧用 `HasMany(...).WithOne(...)` 声明一次，从属侧不重复。
6. **生成迁移后必须 review `Up()`**，确认表/列/索引/FK 与预期一致，而不只是"命令没报错"。
7. **不自动迁移**：启动时不执行 `Database.Migrate()`。

## 后果

好处：

- 把"编译期看不见的失败"提前到秒级反馈。
- 迁移脚本可复现，FK 与索引名稳定。
- 新增实体时有明确检查点，AI 也能遵守。

代价与约束：

- 每次改模型都要跑测试，比只跑 build 多几秒。
- 无 DbSet 但有 EF 配置的实体会被 `ApplyConfigurationsFromAssembly` 自动带入模型；需要显式管理，否则会引入意外的表或表名冲突。
- 迁移文件需要人工 review，不能盲信命令输出。

## 替代方案

- **只依赖 `dotnet build`**：已被证伪，编译绿与模型可用无关。
- **运行时才验证**：把问题推到第一次请求，代价最高，否决。
- **关闭 `ApplyConfigurationsFromAssembly` 改为手工注册配置**：控制更精确，但增加维护成本；若表名冲突反复出现，可升级为此方案。

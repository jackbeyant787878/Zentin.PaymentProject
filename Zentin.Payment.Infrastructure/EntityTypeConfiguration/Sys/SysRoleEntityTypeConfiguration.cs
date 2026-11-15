using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysRoleEntityTypeConfiguration : IEntityTypeConfiguration<SysRole>
    {
        public void Configure(EntityTypeBuilder<SysRole> builder)
        {
            builder.ToTable("SysRole");
            builder.HasKey(e => e.RoleId);

            builder.Property(e => e.RoleId).HasMaxLength(32).ValueGeneratedNever();
            builder.Property(e => e.RoleName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BelongInfoId).HasMaxLength(64).HasDefaultValue("0").IsRequired();

            builder.HasIndex(e => new { e.SysType, e.BelongInfoId }).HasDatabaseName("IdxSysTypeBelongInfoId");
        }
    }
}
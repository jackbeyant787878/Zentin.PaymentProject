using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysUserRoleRelaEntityTypeConfiguration : IEntityTypeConfiguration<SysUserRoleRela>
    {
        public void Configure(EntityTypeBuilder<SysUserRoleRela> builder)
        {
            builder.ToTable("SysUserRoleRela");
            builder.HasKey(e => new { e.UserId, e.RoleId });

            builder.Property(e => e.UserId).HasColumnType("bigint");
            builder.Property(e => e.RoleId).HasMaxLength(32);

            builder.HasIndex(e => e.RoleId).HasDatabaseName("IdxRoleId");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysRoleEntRelaEntityTypeConfiguration : IEntityTypeConfiguration<SysRoleEntRela>
    {
        public void Configure(EntityTypeBuilder<SysRoleEntRela> builder)
        {
            builder.ToTable("SysRoleEntRela");
            builder.HasKey(e => new { e.RoleId, e.EntId });

            builder.Property(e => e.RoleId).HasMaxLength(32);
            builder.Property(e => e.EntId).HasMaxLength(64);

            builder.HasIndex(e => e.EntId).HasDatabaseName("IdxEntId");
        }
    }
}
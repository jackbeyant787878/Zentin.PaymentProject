using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysEntitlementEntityTypeConfiguration : IEntityTypeConfiguration<SysEntitlement>
    {
        public void Configure(EntityTypeBuilder<SysEntitlement> builder)
        {
            builder.ToTable("SysEntitlement");
            builder.HasKey(e => new { e.EntId, e.SysType });

            builder.Property(e => e.EntId).HasMaxLength(64);
            builder.Property(e => e.EntName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.MenuIcon).HasMaxLength(32);
            builder.Property(e => e.MenuUri).HasMaxLength(128);
            builder.Property(e => e.ComponentName).HasMaxLength(32);
            builder.Property(e => e.EntType).HasMaxLength(2).IsFixedLength().IsRequired();
            builder.Property(e => e.QuickJump).HasColumnType("tinyint").HasDefaultValue((byte)0);
            builder.Property(e => e.MatchRule).HasColumnType("nvarchar(500)");
            builder.Property(e => e.State).HasColumnType("tinyint").HasDefaultValue((byte)1);
            builder.Property(e => e.Pid).HasMaxLength(32).IsRequired();
            builder.Property(e => e.EntSort).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.SysType).HasMaxLength(8);

            builder.HasIndex(e => new { e.SysType, e.Pid }).HasDatabaseName("IdxSysTypePid");
        }
    }
}
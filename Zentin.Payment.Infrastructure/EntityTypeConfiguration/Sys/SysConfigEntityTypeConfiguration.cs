using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysConfigEntityTypeConfiguration : IEntityTypeConfiguration<SysConfig>
    {
        public void Configure(EntityTypeBuilder<SysConfig> builder)
        {
            builder.ToTable("SysConfig");
            builder.HasKey(e => new { e.ConfigKey, e.SysType, e.BelongInfoId });

            builder.Property(e => e.ConfigKey).HasMaxLength(50);
            builder.Property(e => e.ConfigName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.ConfigDesc).HasMaxLength(200).IsRequired();
            builder.Property(e => e.GroupKey).HasMaxLength(50).IsRequired();
            builder.Property(e => e.GroupName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.ConfigVal).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(e => e.Type).HasMaxLength(20).HasDefaultValue("text").IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8);
            builder.Property(e => e.BelongInfoId).HasMaxLength(64);
            builder.Property(e => e.SortNum).HasColumnType("bigint").HasDefaultValue(0L);

            builder.HasIndex(e => new { e.SysType, e.BelongInfoId, e.GroupKey })
                .HasDatabaseName("IdxSysTypeBelongInfoIdGroupKey");
        }
    }
}
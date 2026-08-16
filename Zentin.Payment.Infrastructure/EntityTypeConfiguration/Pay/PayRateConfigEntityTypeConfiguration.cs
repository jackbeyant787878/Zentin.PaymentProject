using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayRateConfigEntityTypeConfiguration : IEntityTypeConfiguration<PayRateConfig>
    {
        public void Configure(EntityTypeBuilder<PayRateConfig> builder)
        {
            builder.ToTable("PayRateConfig");
            builder.HasKey(e => e.PayRateConfigId);

            builder.Property(e => e.PayRateConfigId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.ConfigType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.InfoType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.InfoId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.FeeType).HasConversion<int>();
            builder.Property(e => e.LevelMode).HasConversion<int>();
            builder.Property(e => e.FeeRate).HasPrecision(20, 6);
            builder.Property(e => e.ApplymentSupport).HasColumnType("bit");
            builder.Property(e => e.State).HasColumnType("bit");

           
            

            builder.HasIndex(e => new { e.ConfigType, e.InfoType, e.InfoId, e.IfCode, e.WayCode })
                .HasDatabaseName("UniConfigTypeInfoTypeInfoIdIfCodeWayCode")
                .IsUnique();

            // one-to-many: RateConfig → LevelConfig
            builder.HasMany(e => e.LevelConfigs)
                .WithOne(e => e.RateConfig)
                .HasForeignKey(e => e.RateConfigId)
                .HasConstraintName("FKPayRateLevelConfigRateConfigId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

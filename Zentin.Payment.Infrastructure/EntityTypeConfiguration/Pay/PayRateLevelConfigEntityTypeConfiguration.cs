using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayRateLevelConfigConfiguration : IEntityTypeConfiguration<PayRateLevelConfig>
    {
        public void Configure(EntityTypeBuilder<PayRateLevelConfig> builder)
        {
            builder.ToTable("PayRateLevelConfig");
            builder.HasKey(e => e.PayRateLevelConfigId);

            builder.Property(e => e.PayRateLevelConfigId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.RateConfigId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.BankCardType).HasConversion<int>();
            builder.Property(e => e.MinAmount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.MaxAmount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.MinFee).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.MaxFee).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.FeeRate).HasPrecision(20, 6).IsRequired();
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);

           
            
        }
    }
}

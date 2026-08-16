using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayOrderProfitConfiguration : IEntityTypeConfiguration<PayOrderProfit>
    {
        public void Configure(EntityTypeBuilder<PayOrderProfit> builder)
        {
            builder.ToTable("PayOrderProfit");
            builder.HasKey(e => e.PayOrderProfitId);

            builder.Property(e => e.PayOrderProfitId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.InfoId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.InfoName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.InfoType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.PayOrderId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.FeeRate).HasPrecision(20, 6).IsRequired();
            builder.Property(e => e.FeeRateDesc).HasMaxLength(128);
            builder.Property(e => e.FeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.OrderFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ProfitRate).HasPrecision(20, 6).IsRequired();
            builder.Property(e => e.ProfitAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.OrderProfitAmount).HasColumnType("bigint").IsRequired();

           
            

            // unique index: InfoId + InfoType + PayOrderId
            builder.HasIndex(e => new { e.InfoId, e.InfoType, e.PayOrderId })
                .HasDatabaseName("UniInfoIdInfoTypePayOrderId")
                .IsUnique();

            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("IdxCreatedOnUtc");

            // foreign key: PayOrderId
            builder.HasOne(e => e.PayOrder)
                .WithMany(e => e.ProfitRecords)
                .HasForeignKey(e => e.PayOrderId)
                .HasConstraintName("FKPayOrderProfitPayOrderId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.CheckBill;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.CheckBill
{
    public sealed class ChannelBillEntityTypeConfiguration : IEntityTypeConfiguration<ChannelBill>
    {
        public void Configure(EntityTypeBuilder<ChannelBill> builder)
        {
            builder.ToTable("ChannelBill");
            builder.HasKey(e => e.ChannelBillId);

            builder.Property(e => e.ChannelBillId).HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.BatchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.BillType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BillDate).HasColumnType("datetime2(6)").IsRequired();
            builder.Property(e => e.OrderId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.ChannelMchNo).HasMaxLength(64);
            builder.Property(e => e.ChannelOrderNo).HasMaxLength(64);
            builder.Property(e => e.ChannelState).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.ChannelAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelSuccessTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.ChannelUser).HasMaxLength(64);
            builder.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("CNY");

           
            

            builder.HasIndex(e => e.BatchNo).HasDatabaseName("Idx_BatchNo");
            builder.HasIndex(e => e.OrderId).HasDatabaseName("Idx_OrderId");
        }
    }
}

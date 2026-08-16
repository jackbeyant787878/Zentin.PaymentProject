using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.CheckBill;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.CheckBill
{
    public sealed class CheckBillBatchConfiguration : IEntityTypeConfiguration<CheckBillBatch>
    {
        public void Configure(EntityTypeBuilder<CheckBillBatch> builder)
        {
            builder.ToTable("CheckBillBatch");
            builder.HasKey(e => e.BatchNo);

            builder.Property(e => e.BatchNo).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.BillDate).HasColumnType("datetime2(6)").IsRequired();
            builder.Property(e => e.OrgBillFilePath).HasMaxLength(128);
            builder.Property(e => e.ParseStatus).HasConversion<int>().HasDefaultValue(CheckBillParseStatus.Failed);
            builder.Property(e => e.ParseErrMsg).HasMaxLength(256);
            builder.Property(e => e.HandleState).HasConversion<int>().IsRequired();
            builder.Property(e => e.DiffCount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.UnHandleCount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.TotalAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.TotalCount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.TotalFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.TotalRefundAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.TotalRefundCount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.ChannelMchNo).HasMaxLength(64);
            builder.Property(e => e.ChannelTotalAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelTotalCount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.ChannelTotalFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelTotalRefundAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelTotalRefundCount).HasColumnType("int").HasDefaultValue(0);

           
            
        }
    }
}

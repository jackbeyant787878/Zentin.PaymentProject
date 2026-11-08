using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayOrderDivisionRecordConfiguration : IEntityTypeConfiguration<PayOrderDivisionRecord>
    {
        public void Configure(EntityTypeBuilder<PayOrderDivisionRecord> builder)
        {
            builder.ToTable("PayOrderDivisionRecord");
            builder.HasKey(e => e.PayOrderDivisionRecordId);

            builder.Property(e => e.PayOrderDivisionRecordId)
                .HasColumnType("bigint")
                .ValueGeneratedNever();

            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchName).HasMaxLength(30).IsRequired();
            builder.Property(e => e.MchType).HasColumnType("int").IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.PayOrderId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.PayOrderChannelOrderNo).HasMaxLength(64);
            builder.Property(e => e.PayOrderAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.PayOrderDivisionAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.BatchOrderId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.ChannelBatchOrderId).HasMaxLength(64);
            builder.Property(e => e.State).HasConversion<int>().IsRequired();
            builder.Property(e => e.ChannelRespResult).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ReceiverId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ReceiverGroupId).HasColumnType("bigint");
            builder.Property(e => e.ReceiverAlias).HasMaxLength(64);
            builder.Property(e => e.AccType).HasConversion<int>().IsRequired();
            builder.Property(e => e.AccNo).HasMaxLength(50).IsRequired();
            builder.Property(e => e.AccName).HasMaxLength(30).HasDefaultValue("").IsRequired();
            builder.Property(e => e.RelationType).HasMaxLength(30).IsRequired();
            builder.Property(e => e.RelationTypeName).HasMaxLength(30).IsRequired();
            builder.Property(e => e.DivisionProfit).HasPrecision(20, 6).IsRequired();
            builder.Property(e => e.CalDivisionAmount).HasColumnType("bigint").IsRequired();

           
            

            builder.HasOne(e => e.PayOrder).WithMany(o => o.DivisionRecords).HasForeignKey(e => e.PayOrderId).OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PayOrder_DivisionRecord");

            builder.HasIndex(e => e.PayOrderId).HasDatabaseName("Idx_PayOrderId");
            builder.HasIndex(e => e.BatchOrderId).HasDatabaseName("Idx_BatchOrderId");
            builder.HasIndex(e => e.State).HasDatabaseName("Idx_State");
        }
    }
}

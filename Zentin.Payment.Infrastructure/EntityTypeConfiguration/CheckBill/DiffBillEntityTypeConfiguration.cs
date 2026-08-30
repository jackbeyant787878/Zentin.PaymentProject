using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.CheckBill;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.CheckBill
{
    public sealed class DiffBillConfiguration : IEntityTypeConfiguration<DiffBill>
    {
        public void Configure(EntityTypeBuilder<DiffBill> builder)
        {
            builder.ToTable("DiffBill");
            builder.HasKey(e => e.DiffBillId);

            builder.Property(e => e.DiffBillId).HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.DiffType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.BatchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.BillType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BillDate).HasColumnType("datetime2(6)").IsRequired();
            builder.Property(e => e.OrderId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.OrderState).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.OrderSuccessTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.OrderCreatedAt).HasColumnType("datetime2(6)").IsRequired();
            builder.Property(e => e.Amount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.FeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.RefundAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelMchNo).HasMaxLength(64);
            builder.Property(e => e.ChannelOrderNo).HasMaxLength(64);
            builder.Property(e => e.ChannelAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelRefundAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.ChannelSuccessTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.HandleState).HasConversion<int>().HasDefaultValue(DiffBillHandleState.Unhandled);
            builder.Property(e => e.HandleUid).HasColumnType("bigint");
            builder.Property(e => e.HandleBy).HasMaxLength(64);




            builder.HasIndex(e => e.BatchNo).HasDatabaseName("Idx_BatchNo");
            builder.HasIndex(e => e.HandleState).HasDatabaseName("Idx_HandleState");
        }
    }
}

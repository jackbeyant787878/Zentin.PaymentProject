using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration
{
    public sealed class PayOrderEntityTypeConfiguration : IEntityTypeConfiguration<PayOrder>
    {
        public void Configure(EntityTypeBuilder<PayOrder> builder)
        {
            builder.ToTable("PayOrder");
            builder.HasKey(e => e.PayOrderId);

            builder.Property(e => e.PayOrderId).HasMaxLength(30).ValueGeneratedNever();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchShortName).HasMaxLength(32);
            builder.Property(e => e.AgentNo).HasMaxLength(64);
            builder.Property(e => e.AgentName).HasMaxLength(64);
            builder.Property(e => e.AgentShortName).HasMaxLength(32);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.IsvName).HasMaxLength(64);
            builder.Property(e => e.IsvShortName).HasMaxLength(32);
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AppName).HasMaxLength(64);
            builder.Property(e => e.StoreId).HasColumnType("bigint");
            builder.Property(e => e.StoreName).HasMaxLength(64);
            builder.Property(e => e.QrcId).HasMaxLength(64);
            builder.Property(e => e.MchType).HasColumnType("int").IsRequired();
            builder.Property(e => e.MchOrderNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20);
            builder.Property(e => e.WayCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Amount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.MchFeeRate).HasPrecision(20, 6).IsRequired();
            builder.Property(e => e.MchFeeRateDesc).HasMaxLength(128);
            builder.Property(e => e.MchFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.MchOrderFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("CNY").IsRequired();
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(PayOrderStatus.Created);
            builder.Property(e => e.NotifyState).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.ClientIp).HasMaxLength(32);
            builder.Property(e => e.Subject).HasMaxLength(64).IsRequired();
            builder.Property(e => e.Body).HasMaxLength(256).IsRequired();
            builder.Property(e => e.SellerRemark).HasMaxLength(256);
            builder.Property(e => e.BuyerRemark).HasMaxLength(256);
            builder.Property(e => e.ChannelMchNo).HasMaxLength(64);
            builder.Property(e => e.ChannelIsvNo).HasMaxLength(64);
            builder.Property(e => e.ChannelExtra).HasMaxLength(512);
            builder.Property(e => e.ChannelUser).HasMaxLength(64);
            builder.Property(e => e.ChannelOrderNo).HasMaxLength(64);
            builder.Property(e => e.PlatformOrderNo).HasMaxLength(64);
            builder.Property(e => e.PlatformMchOrderNo).HasMaxLength(64);
            builder.Property(e => e.RefundState).HasConversion<int>().HasDefaultValue(RefundStatus.NoRefund);
            builder.Property(e => e.RefundTimes).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.RefundAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.DivisionMode).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.DivisionState).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.DivisionLastTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.ErrCode).HasMaxLength(128);
            builder.Property(e => e.ErrMsg).HasMaxLength(256);
            builder.Property(e => e.ExtParam).HasMaxLength(128);
            builder.Property(e => e.NotifyUrl).HasMaxLength(128).HasDefaultValue("").IsRequired();
            builder.Property(e => e.ReturnUrl).HasMaxLength(128).HasDefaultValue("");
            builder.Property(e => e.ExpiredTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.SuccessTime).HasColumnType("datetime2(6)");

            // audit fields
           
            

            // indexes
            builder.HasIndex(e => new { e.MchNo, e.MchOrderNo }).HasDatabaseName("UniMchNoMchOrderNo").IsUnique();
            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("IdxCreatedOnUtc");
            builder.HasIndex(e => e.State).HasDatabaseName("IdxState");

            
            //one-to-many relationship: PayOrder → RefundOrders
            builder.HasMany(e => e.RefundOrders)
                .WithOne(e => e.PayOrder)
                .HasForeignKey(e => e.PayOrderId)
                .HasConstraintName("FKRefundOrderPayOrderId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

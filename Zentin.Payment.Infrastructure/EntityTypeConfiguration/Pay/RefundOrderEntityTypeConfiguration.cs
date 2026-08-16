using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Order;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{

    public sealed class RefundOrderConfiguration : IEntityTypeConfiguration<RefundOrder>
    {
        public void Configure(EntityTypeBuilder<RefundOrder> builder)
        {
            builder.ToTable("RefundOrder");
            builder.HasKey(e => e.RefundOrderId);

            builder.Property(e => e.RefundOrderId) .HasMaxLength(30).ValueGeneratedNever();
            builder.Property(e => e.State) .HasConversion<byte>().HasDefaultValue((byte)RefundOrderStatus.Created) .IsRequired();

         
            builder.Property(e => e.PayOrderId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.ChannelPayOrderNo).HasMaxLength(64);
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
            builder.Property(e => e.MchType).HasColumnType("int").IsRequired();
            builder.Property(e => e.MchRefundNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.WayCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.PayAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.RefundAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.RefundFeeAmount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("CNY");
            builder.Property(e => e.ClientIp).HasMaxLength(32);
            builder.Property(e => e.RefundReason).HasMaxLength(256).IsRequired();
            builder.Property(e => e.ChannelOrderNo).HasMaxLength(32);
            builder.Property(e => e.ErrCode).HasMaxLength(128);
            builder.Property(e => e.ErrMsg).HasMaxLength(2048);
            builder.Property(e => e.ChannelExtra).HasMaxLength(512);
            builder.Property(e => e.NotifyUrl).HasMaxLength(128);
            builder.Property(e => e.ExtParam).HasMaxLength(64);
            builder.Property(e => e.SuccessTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.ExpiredTime).HasColumnType("datetime2(6)");

           

            //indexes and constraints
            builder.HasIndex(e => new { e.MchNo, e.MchRefundNo }).HasDatabaseName("Uni_MchNo_MchRefundNo") .IsUnique();
            builder.HasIndex(e => e.PayOrderId).HasDatabaseName("Idx_PayOrderId");
            builder.HasIndex(e => e.State).HasDatabaseName("Idx_State");

            //foreign key relationship with PayOrder
            builder.HasOne(e => e.PayOrder)
                .WithMany(o => o.RefundOrders)
                .HasForeignKey(e => e.PayOrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RefundOrder_PayOrder");
        }
    }


}

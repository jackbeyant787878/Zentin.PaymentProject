using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Member
{

    /// <summary>
    /// Member Recharge Order Configuration
    /// </summary>
    public sealed class MbrRechargeOrderEntityTypeConfiguration : IEntityTypeConfiguration<MbrRechargeOrder>
    {
        public void Configure(EntityTypeBuilder<MbrRechargeOrder> builder)
        {
            builder.ToTable("MbrRechargeOrder");
            builder.HasKey(e => e.RechargeOrderId);

            builder.Property(e => e.RechargeOrderId).HasMaxLength(30).ValueGeneratedNever();
            builder.Property(e => e.MbrNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MbrName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MbrTel).HasMaxLength(32);
            builder.Property(e => e.PayOrderId).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.WayCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayName).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.PayAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.EntryAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.GiveAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.AfterBalance).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.AvatarUrl).HasMaxLength(128);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(MbrRechargeState.Init);
            builder.Property(e => e.NotifyUrl).HasMaxLength(128).HasDefaultValue("").IsRequired();
            builder.Property(e => e.ReturnUrl).HasMaxLength(128).HasDefaultValue("");
            builder.Property(e => e.ExpiredTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.SuccessTime).HasColumnType("datetime2(6)");

           
            

            builder.HasIndex(e => e.MbrNo).HasDatabaseName("IdxMbrNo");
            builder.HasIndex(e => e.MchNo).HasDatabaseName("IdxMchNo");
            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("IdxCreatedOnUtc");
        }
    }
}

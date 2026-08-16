using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayInterfaceDefineEntityTypeConfiguration : IEntityTypeConfiguration<PayInterfaceDefine>
    {
        public void Configure(EntityTypeBuilder<PayInterfaceDefine> builder)
        {
            builder.ToTable("PayInterfaceDefine");
            builder.HasKey(e => e.IfCode);

            builder.Property(e => e.IfCode).HasMaxLength(20).ValueGeneratedNever();
            builder.Property(e => e.IfName).HasMaxLength(20).IsRequired();
            builder.Property(e => e.IsMchMode).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.IsIsvMode).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.ConfigPageType).HasConversion<int>().HasDefaultValue(ConfigPageType.JsonRender);
            builder.Property(e => e.IsSupportApplyment).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsOpenApplyment).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsSupportCheckBill).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsOpenCheckBill).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsSupportCashout).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsOpenCashout).HasColumnType("bit").HasDefaultValue(false);

            builder.Property(e => e.IsvParams).HasMaxLength(4096);
            builder.Property(e => e.IsvsubMchParams).HasMaxLength(4096);
            builder.Property(e => e.NormalMchParams).HasMaxLength(4096);
            builder.Property(e => e.WayCodes).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(e => e.Icon).HasMaxLength(256);
            builder.Property(e => e.BgColor).HasMaxLength(20);
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.Remark).HasMaxLength(128);

           
            
        }
    }
}

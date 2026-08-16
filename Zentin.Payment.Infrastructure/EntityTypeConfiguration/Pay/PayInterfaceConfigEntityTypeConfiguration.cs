using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayInterfaceConfigConfiguration : IEntityTypeConfiguration<PayInterfaceConfig>
    {
        public void Configure(EntityTypeBuilder<PayInterfaceConfig> builder)
        {
            builder.ToTable("PayInterfaceConfig");
            builder.HasKey(e => e.PayInterfaceConfigId);

            builder.Property(e => e.PayInterfaceConfigId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.InfoType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.InfoId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.IfParams).HasMaxLength(4096).IsRequired();
            builder.Property(e => e.SettHoldDay).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.IfRate).HasPrecision(20, 6);
            builder.Property(e => e.IsOpenApplyment).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsOpenCashout).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IsOpenCheckBill).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.IgnoreCheckBillMchNos).HasMaxLength(4096);
            builder.Property(e => e.Oauth2InfoId).HasMaxLength(20);
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);
            builder.Property(e => e.UpdatedUid).HasColumnType("bigint");
            builder.Property(e => e.UpdatedBy).HasMaxLength(64);

           
            

            builder.HasIndex(e => new { e.InfoType, e.InfoId, e.IfCode })
                .HasDatabaseName("UniInfoTypeInfoIdIfCode")
                .IsUnique();
        }
    }
}

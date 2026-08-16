using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Notify;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Notify
{
    public sealed class MchNotifyRecordConfiguration : IEntityTypeConfiguration<MchNotifyRecord>
    {
        public void Configure(EntityTypeBuilder<MchNotifyRecord> builder)
        {
            builder.ToTable("MchNotifyRecord");
            builder.HasKey(e => e.NotifyId);

            builder.Property(e => e.NotifyId).HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.OrderId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.OrderType).HasConversion<int>().IsRequired();
            builder.Property(e => e.MchOrderNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AgentNo).HasMaxLength(64);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.NotifyUrl).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(e => e.ReqMethod).HasMaxLength(10).IsRequired();
            builder.Property(e => e.ReqMediaType).HasMaxLength(64).IsRequired();
            builder.Property(e => e.ReqBody).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ResResult).HasColumnType("nvarchar(max)");
            builder.Property(e => e.NotifyCount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.NotifyCountLimit).HasColumnType("int").HasDefaultValue(6);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(NotifyState.Notifying);
            builder.Property(e => e.LastNotifyTime).HasColumnType("datetime2(6)");

            builder.HasIndex(e => new { e.OrderId, e.OrderType }).HasDatabaseName("Uni_OrderId_OrderType").IsUnique();
        }
    }
}

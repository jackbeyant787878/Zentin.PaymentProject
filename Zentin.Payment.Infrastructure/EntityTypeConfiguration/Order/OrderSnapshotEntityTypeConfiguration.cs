using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Order;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Order
{
    public sealed class OrderSnapshotEntityTypeConfiguration : IEntityTypeConfiguration<OrderSnapshot>
    {
        public void Configure(EntityTypeBuilder<OrderSnapshot> builder)
        {
            builder.ToTable("OrderSnapshot");
            builder.HasKey(e => new { e.OrderId, e.OrderType });

            builder.Property(e => e.OrderId).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.OrderType).HasConversion<int>().ValueGeneratedNever();

            builder.Property(e => e.MchReqData).HasColumnType("nvarchar(max)");
            builder.Property(e => e.MchReqTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.MchRespData).HasColumnType("nvarchar(max)");
            builder.Property(e => e.MchRespTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.ChannelReqData).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ChannelReqTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.ChannelRespData).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ChannelRespTime).HasColumnType("datetime2(6)");

           
            builder.Property(e => e.UpdatedOnUtc).HasColumnType("datetime2(6)") .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}

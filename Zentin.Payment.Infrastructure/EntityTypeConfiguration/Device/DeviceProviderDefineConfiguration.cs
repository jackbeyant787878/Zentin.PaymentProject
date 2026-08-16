using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Device;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Device
{
    public sealed class DeviceProviderDefineEntityConfiguration : IEntityTypeConfiguration<DeviceProviderDefine>
    {
        public void Configure(EntityTypeBuilder<DeviceProviderDefine> builder)
        {
            builder.ToTable("DeviceProviderDefine");
            builder.HasKey(e => e.ProviderCode);

            builder.Property(e => e.ProviderCode).HasMaxLength(20).ValueGeneratedNever();
            builder.Property(e => e.ProviderName).HasMaxLength(20).IsRequired();
            builder.Property(e => e.ConfigPageType).HasConversion<int>().HasDefaultValue(ConfigPageType.JsonRender);
            builder.Property(e => e.ProviderParams).HasMaxLength(4096);
            builder.Property(e => e.DeviceTypes).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(e => e.Icon).HasMaxLength(256);
            builder.Property(e => e.BgColor).HasMaxLength(20);
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.Remark).HasMaxLength(128);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Device;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Device
{

    public sealed class DeviceProviderConfigConfiguration : IEntityTypeConfiguration<DeviceProviderConfig>
    {
        public void Configure(EntityTypeBuilder<DeviceProviderConfig> builder)
        {
            builder.ToTable("DeviceProviderConfig");
            builder.HasKey(e => e.DeviceProviderConfigId);

            builder.Property(e => e.DeviceProviderConfigId).HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.DeviceType).HasConversion<int>().IsRequired();
            builder.Property(e => e.ProviderCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.ProviderParams).HasMaxLength(4096).IsRequired();
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.Remark).HasMaxLength(128);

           
            

            builder.HasIndex(e => new { e.DeviceType, e.ProviderCode }).HasDatabaseName("Uni_DeviceType_Provider").IsUnique();
        }
    }
}

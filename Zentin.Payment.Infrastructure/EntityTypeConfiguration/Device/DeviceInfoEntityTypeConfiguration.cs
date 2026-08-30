using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Device;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Device
{
    public sealed class DeviceInfoEntityTyConfiguration : IEntityTypeConfiguration<DeviceInfo>
    {
        public void Configure(EntityTypeBuilder<DeviceInfo> builder)
        {
            builder.ToTable("DeviceInfo");
            builder.HasKey(e => e.DeviceInfoId);

            builder.Property(e => e.DeviceInfoId).HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.DeviceNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.DeviceName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.ConfigId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.DeviceType).HasConversion<int>().IsRequired();
            builder.Property(e => e.ProviderCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.BatchId).HasMaxLength(64);
            builder.Property(e => e.BindState).HasConversion<int>().IsRequired();
            builder.Property(e => e.BindType).HasConversion<int>().IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.StoreId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.BindQrcId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BelongInfoId).HasMaxLength(64).IsRequired();

            builder.Property(e => e.CreatedOnUtc).HasColumnType("datetime2(6)").HasDefaultValueSql("GETUTCDATE()");


            builder.HasIndex(e => new { e.ProviderCode, e.DeviceType, e.DeviceNo }).HasDatabaseName("Uni_ProviderCode_DeviceType_DeviceNo").IsUnique();

            builder.HasIndex(e => e.MchNo).HasDatabaseName("Idx_MchNo");
            builder.HasIndex(e => e.BindState).HasDatabaseName("Idx_BindState");
        }
    }
}

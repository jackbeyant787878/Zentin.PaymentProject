using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Merchant;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Merchant
{
    public sealed class MchStoreEntityTypeConfiguration : IEntityTypeConfiguration<MchStore>
    {
        public void Configure(EntityTypeBuilder<MchStore> builder)
        {
            builder.ToTable("MchStore");
            builder.HasKey(e => e.StoreId);

            builder.Property(e => e.StoreId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.StoreName).HasMaxLength(64).HasDefaultValue("").IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AgentNo).HasMaxLength(64);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.ContactPhone).HasMaxLength(32).IsRequired();
            builder.Property(e => e.StoreLogo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.StoreOuterImg).HasMaxLength(64).IsRequired();
            builder.Property(e => e.StoreInnerImg).HasMaxLength(64).IsRequired();
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.ProvinceCode).HasMaxLength(32).IsRequired();
            builder.Property(e => e.CityCode).HasMaxLength(32).IsRequired();
            builder.Property(e => e.DistrictCode).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Address).HasMaxLength(128).IsRequired();
            builder.Property(e => e.Lng).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Lat).HasMaxLength(32).IsRequired();
            builder.Property(e => e.DefaultFlag).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.BindAppId).HasMaxLength(64);
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

            builder.HasIndex(e => e.MchNo).HasDatabaseName("IdxMchNo");
        }
    }
}
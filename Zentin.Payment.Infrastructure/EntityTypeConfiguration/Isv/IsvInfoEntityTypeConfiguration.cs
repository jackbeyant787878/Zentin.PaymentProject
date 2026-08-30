using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Isv;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Isv
{
    public sealed class IsvInfoEntityTypeConfiguration : IEntityTypeConfiguration<IsvInfo>
    {
        public void Configure(EntityTypeBuilder<IsvInfo> builder)
        {
            builder.ToTable("IsvInfo");
            builder.HasKey(e => e.IsvNo);

            builder.Property(e => e.IsvNo).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.IsvName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IsvShortName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.ContactName).HasMaxLength(32);
            builder.Property(e => e.ContactTel).HasMaxLength(32);
            builder.Property(e => e.ContactEmail).HasMaxLength(32);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(IsvState.Normal);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

        }
    }
}

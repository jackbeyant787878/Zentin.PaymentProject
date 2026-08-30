using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Member
{
    /// <summary>
    ///Member Bill Configuration
    /// </summary>
    public sealed class MbrBillEntityTypeConfiguration : IEntityTypeConfiguration<MbrBill>
    {
        public void Configure(EntityTypeBuilder<MbrBill> builder)
        {
            builder.ToTable("MbrBill");
            builder.HasKey(e => e.MbrBillId);

            builder.Property(e => e.MbrBillId).HasMaxLength(30).ValueGeneratedNever();
            builder.Property(e => e.MbrNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MbrName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MbrTel).HasMaxLength(32);
            builder.Property(e => e.BeforeBalance).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.ChangeAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.AfterBalance).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.BizType).HasConversion<int>().HasDefaultValue(MbrBillBizType.PayRecharge);
            builder.Property(e => e.Remark).HasMaxLength(128);




            builder.HasIndex(e => e.MbrNo).HasDatabaseName("IdxMbrNo");
            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("IdxCreatedOnUtc");
        }
    }
}

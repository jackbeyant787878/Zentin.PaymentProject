using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Finance;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Finance
{
    public sealed class AccountBillConfiguration : IEntityTypeConfiguration<AccountBill>
    {
        public void Configure(EntityTypeBuilder<AccountBill> builder)
        {
            builder.ToTable("AccountBill");
            builder.HasKey(e => e.AccountId);

            builder.Property(e => e.AccountId)
                .HasColumnType("bigint")
                .ValueGeneratedNever();

            builder.Property(e => e.BillId).HasMaxLength(30).IsRequired();
            builder.Property(e => e.InfoId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.InfoName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.InfoType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.BeforeBalance).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.ChangeAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.AfterBalance).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.BizType).HasConversion<int>().HasDefaultValue(AccountBillBizType.OrderCommission);
            builder.Property(e => e.AccountType).HasConversion<int>().HasDefaultValue(AccountType.Wallet);
            builder.Property(e => e.RelaBizOrderType).HasConversion<int>();
            builder.Property(e => e.RelaBizOrderId).HasMaxLength(30);
            builder.Property(e => e.Remark).HasMaxLength(128);

        
            builder.HasIndex(e => e.BillId).HasDatabaseName("UniBillId").IsUnique();
            builder.HasIndex(e => new { e.InfoId, e.InfoType }).HasDatabaseName("IdxInfoIdInfoType");
            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("IdxCreatedOnUtc");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class TransferOrderEntityTypeConfiguration : IEntityTypeConfiguration<TransferOrder>
    {
        public void Configure(EntityTypeBuilder<TransferOrder> builder)
        {
            builder.ToTable("TransferOrder");
            builder.HasKey(e => e.TransferId);

            builder.Property(e => e.TransferId).HasMaxLength(32).ValueGeneratedNever();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchShortName).HasMaxLength(32);
            builder.Property(e => e.AgentNo).HasMaxLength(64);
            builder.Property(e => e.AgentName).HasMaxLength(64);
            builder.Property(e => e.AgentShortName).HasMaxLength(32);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.IsvName).HasMaxLength(64);
            builder.Property(e => e.IsvShortName).HasMaxLength(32);
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchType).HasColumnType("int").IsRequired();
            builder.Property(e => e.MchOrderNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.EntryType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Amount).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("CNY");
            builder.Property(e => e.AccountNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AccountName).HasMaxLength(64);
            builder.Property(e => e.BankName).HasMaxLength(32);
            builder.Property(e => e.TransferDesc).HasMaxLength(128).HasDefaultValue("");
            builder.Property(e => e.ClientIp).HasMaxLength(32);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(TransferOrderStatus.Created);
            builder.Property(e => e.ChannelExtra).HasMaxLength(512);
            builder.Property(e => e.ChannelOrderNo).HasMaxLength(64);
            builder.Property(e => e.ErrCode).HasMaxLength(128);
            builder.Property(e => e.ErrMsg).HasMaxLength(256);
            builder.Property(e => e.ExtParam).HasMaxLength(128);
            builder.Property(e => e.NotifyUrl).HasMaxLength(128).HasDefaultValue("");
            builder.Property(e => e.SuccessTime).HasColumnType("datetime2(6)");

            
            builder.HasIndex(e => new { e.MchNo, e.MchOrderNo }) .HasDatabaseName("Uni_MchNo_MchOrderNo").IsUnique();
            builder.HasIndex(e => e.State).HasDatabaseName("Idx_State");
            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("Idx_CreatedAt");
        }
    }
}

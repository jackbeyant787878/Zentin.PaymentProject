using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Merchant
{
    /// <summary>
    /// 商户信息配置
    /// </summary>
    public sealed class MchInfoEntityTypeConfiguration : IEntityTypeConfiguration<MchInfo>
    {
        public void Configure(EntityTypeBuilder<MchInfo> builder)
        {
            builder.ToTable("MchInfo");
            builder.HasKey(e => e.MchNo);

            builder.Property(e => e.MchNo).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.MchName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchShortName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Type).HasConversion<int>().HasDefaultValue(MchType.Normal);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.MchLevel).HasMaxLength(8).HasDefaultValue("M0").IsRequired();
            

            builder.Property(e => e.RefundMode).HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            ).HasColumnType("nvarchar(500)");

            builder.Property(e => e.Sipw).HasMaxLength(128);
            builder.Property(e => e.TopAgentNo).HasMaxLength(64);
            builder.Property(e => e.AgentNo).HasMaxLength(64);
            builder.Property(e => e.ContactName).HasMaxLength(32);
            builder.Property(e => e.ContactTel).HasMaxLength(32);
            builder.Property(e => e.ContactEmail).HasMaxLength(32);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(MchState.Normal).IsRequired();
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.InitUserId).HasColumnType("bigint");
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

          
            // one-to-many relationship: Merchant → Application
            builder.HasMany(e => e.Apps)
                .WithOne(e => e.MchInfo)
                .HasForeignKey(e => e.MchNo)
                .HasConstraintName("FKMchAppMchNo")
                .OnDelete(DeleteBehavior.Cascade);

            // one-to-many relationship: Merchant → Store
            builder.HasMany(e => e.Stores)
                .WithOne(e => e.MchInfo)
                .HasForeignKey(e => e.MchNo)
                .HasConstraintName("FKMchStoreMchNo")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

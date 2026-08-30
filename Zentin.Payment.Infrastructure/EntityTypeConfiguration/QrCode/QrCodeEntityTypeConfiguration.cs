using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.QrCode
{
    public sealed class QrCodeConfiguration : IEntityTypeConfiguration<Domain.Entities.QrCode.QrCode>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.QrCode.QrCode> builder)
        {
            builder.ToTable("QrCode");
            builder.HasKey(e => e.QrCodeId);

            builder.Property(e => e.QrCodeId).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.QrConstantshellId).HasColumnType("int");
            builder.Property(e => e.BatchId).HasMaxLength(64);
            builder.Property(e => e.FixedFlag).HasColumnType("bit").IsRequired();
            builder.Property(e => e.FixedPayAmount).HasColumnType("int").HasDefaultValue(0);
            builder.Property(e => e.EntryPage).HasMaxLength(20).IsRequired();
            builder.Property(e => e.AlipayWayCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.QrcAlias).HasMaxLength(20);
            builder.Property(e => e.BindState).HasConversion<int>().IsRequired();
            builder.Property(e => e.AgentNo).HasMaxLength(64);
            builder.Property(e => e.MchNo).HasMaxLength(64);
            builder.Property(e => e.AppId).HasMaxLength(64);
            builder.Property(e => e.StoreId).HasColumnType("bigint");
            builder.Property(e => e.UrlPlaceholder).HasMaxLength(20);
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BelongInfoId).HasMaxLength(64).IsRequired();

           
            

            builder.HasIndex(e => e.MchNo).HasDatabaseName("Idx_MchNo");
            builder.HasIndex(e => e.BindState).HasDatabaseName("Idx_BindState");
        }
    }
}

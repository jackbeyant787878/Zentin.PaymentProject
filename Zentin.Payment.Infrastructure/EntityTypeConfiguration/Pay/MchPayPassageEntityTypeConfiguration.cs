using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Pay;
namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class MchPayPassageEntityTypeConfiguration : IEntityTypeConfiguration<MchPayPassage>
    {
        public void Configure(EntityTypeBuilder<MchPayPassage> builder)
        {
            builder.ToTable("MchPayPassage");
            builder.HasKey(e => e.MchPayPassageId);

            builder.Property(e => e.MchPayPassageId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Rate).HasPrecision(20, 6).IsRequired();
            builder.Property(e => e.RiskConfig).HasColumnType("nvarchar(max)");
            builder.Property(e => e.State).HasColumnType("bit").HasDefaultValue(true);

           
            

            builder.HasIndex(e => new { e.AppId, e.IfCode, e.WayCode })
                .HasDatabaseName("UniAppIdIfCodeWayCode")
                .IsUnique();
        }
    }
}

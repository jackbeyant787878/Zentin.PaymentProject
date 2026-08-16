using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    public sealed class PayWayEntityTypeConfiguration : IEntityTypeConfiguration<PayWay>
    {
        public void Configure(EntityTypeBuilder<PayWay> builder)
        {
            builder.ToTable("PayWay");
            builder.HasKey(e => e.WayCode);

            builder.Property(e => e.WayCode).HasMaxLength(20).ValueGeneratedNever();
            builder.Property(e => e.WayName).HasMaxLength(20).IsRequired();
            builder.Property(e => e.WayType).HasMaxLength(20).IsRequired();
            
        }
    }
}

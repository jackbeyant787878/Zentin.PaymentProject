using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Division;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Division
{
    public sealed class MchDivisionReceiverGroupEntityTypeConfiguration : IEntityTypeConfiguration<MchDivisionReceiverGroup>
    {
        public void Configure(EntityTypeBuilder<MchDivisionReceiverGroup> builder)
        {
            builder.ToTable("MchDivisionReceiverGroup");
            builder.HasKey(e => e.ReceiverGroupId);

            builder.Property(e => e.ReceiverGroupId) .HasColumnType("bigint").ValueGeneratedNever();

            builder.Property(e => e.ReceiverGroupName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AutoDivisionFlag).HasColumnType("bit").HasDefaultValue(false);
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

            builder.Property(e => e.CreatedOnUtc) .HasColumnType("datetime2(6)").HasDefaultValueSql("GETUTCDATE()");
            

            builder.HasIndex(e => e.MchNo).HasDatabaseName("Idx_MchNo");
        }
    }
}

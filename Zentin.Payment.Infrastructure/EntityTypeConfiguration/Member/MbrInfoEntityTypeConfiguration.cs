using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Member
{

    /// <summary>
    /// Member Information Configuration
    /// </summary>
    public sealed class MbrInfoEntityTypeConfiguration : IEntityTypeConfiguration<MbrInfo>
    {
        public void Configure(EntityTypeBuilder<MbrInfo> builder)
        {
            builder.ToTable("MbrInfo");
            builder.HasKey(e => e.MbrNo);

            builder.Property(e => e.MbrNo).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.MbrName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MbrTel).HasMaxLength(32);
            builder.Property(e => e.Balance).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.AliUserId).HasMaxLength(64);
            builder.Property(e => e.WxMpOpenId).HasMaxLength(64);
            builder.Property(e => e.AvatarUrl).HasMaxLength(128);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(MbrState.Normal);
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

           
            

            builder.HasIndex(e => e.MchNo).HasDatabaseName("IdxMchNo");
            builder.HasIndex(e => e.MbrTel).HasDatabaseName("IdxMbrTel");

            
            //one-to-many: Member → Bill
            builder.HasMany(e => e.Bills)
                .WithOne(e => e.MbrInfo)
                .HasForeignKey(e => e.MbrNo)
                .HasConstraintName("FKMbrBillMbrNo")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

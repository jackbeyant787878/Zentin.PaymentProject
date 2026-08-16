using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Member
{
    public sealed class MbrRechargeRuleEntityTypeConfiguration : IEntityTypeConfiguration<MbrRechargeRule>
    {
        public void Configure(EntityTypeBuilder<MbrRechargeRule> builder)
        {
            builder.ToTable("MbrRechargeRule");
            builder.HasKey(e => e.RuleId);

            builder.Property(e => e.RuleId)
                .HasColumnType("bigint")
                .ValueGeneratedNever();

            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.RechargeAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.GiveAmount).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.Sort).HasColumnType("bigint").HasDefaultValue(0);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(MbrState.Normal);

       
            builder.HasIndex(e => e.MchNo).HasDatabaseName("IdxMchNo");
        }
    }
}

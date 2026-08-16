using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Merchant
{
    /// <summary>
    /// Merchant Apply Configuration
    /// </summary>
    /// 
    public sealed class MchApplyEntityTypeConfiguration : IEntityTypeConfiguration<MchApply>
    {
        public void Configure(EntityTypeBuilder<MchApply> builder)
        {
            builder.ToTable("MchApply");
            builder.HasKey(e => e.ApplyId);

            builder.Property(e => e.ApplyId).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.AgentNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.TopAgentNo).HasMaxLength(64);
            builder.Property(e => e.IsvNo).HasMaxLength(64);
            builder.Property(e => e.IfCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.AutoConfigMchAppId).HasMaxLength(64).IsRequired();
            builder.Property(e => e.ApplyPageType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.ApplyDetailInfo).HasMaxLength(255).IsRequired();
            builder.Property(e => e.ApplyErrorInfo).HasMaxLength(255).IsRequired();
            builder.Property(e => e.ChannelApplyNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.SuccResParameter).HasMaxLength(255).IsRequired();
            builder.Property(e => e.ChannelVar1).HasMaxLength(255).IsRequired();
            builder.Property(e => e.ChannelVar2).HasMaxLength(255).IsRequired();
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(MchApplyState.Draft);
            builder.Property(e => e.IsTempData).HasColumnType("bit").IsRequired();
            builder.Property(e => e.MchFullName).HasMaxLength(64).IsRequired();
            builder.Property(e => e.MchShortName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.MerchantType).HasConversion<int>().HasDefaultValue(MerchantSubjectType.Personal);
            builder.Property(e => e.ContactName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.ContactPhone).HasMaxLength(32).IsRequired();
            builder.Property(e => e.ContactEmail).HasMaxLength(32);
            builder.Property(e => e.ProvinceCode).HasMaxLength(32).IsRequired();
            builder.Property(e => e.CityCode).HasMaxLength(32).IsRequired();
            builder.Property(e => e.DistrictCode).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Address).HasMaxLength(128).IsRequired();
            builder.Property(e => e.EpUserId).HasColumnType("bigint");
            builder.Property(e => e.LastApplyAt).HasColumnType("datetime2(6)").IsRequired();
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

           
            builder.HasIndex(e => e.MchNo).HasDatabaseName("IdxMchNo");
            builder.HasIndex(e => e.State).HasDatabaseName("IdxState");
        }
    }
}

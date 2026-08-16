using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Merchant
{
    /// <summary>
    /// 商户应用配置
    /// </summary>
    public sealed class MchAppEntityTypeConfiguration : IEntityTypeConfiguration<MchApp>
    {
        public void Configure(EntityTypeBuilder<MchApp> builder)
        {
            builder.ToTable("MchApp");
            builder.HasKey(e => e.AppId);

            builder.Property(e => e.AppId).HasMaxLength(64).ValueGeneratedNever();
            builder.Property(e => e.AppName).HasMaxLength(64).HasDefaultValue("").IsRequired();
            builder.Property(e => e.MchNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(MchAppState.Normal);
            builder.Property(e => e.DefaultFlag).HasColumnType("int").HasDefaultValue(0);

            builder.Property(e => e.AppSignType) .HasConversion(
              v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
              v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
          )
          .HasColumnType("nvarchar(500)"); 

            builder.Property(e => e.AppSecret).HasMaxLength(128).IsRequired();
            builder.Property(e => e.AppRsa2PublicKey).HasMaxLength(448);
            builder.Property(e => e.Remark).HasMaxLength(128);
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

           
            

            builder.HasIndex(e => e.MchNo).HasDatabaseName("IdxMchNo");
        }
    }
}

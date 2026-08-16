using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.QrCode;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.QrCode
{
    public sealed class QrCodeShellEntityTypeConfiguration : IEntityTypeConfiguration<QrCodeShell>
    {
        public void Configure(EntityTypeBuilder<QrCodeShell> builder)
        {
            builder.ToTable("QrCodeShell");
            builder.HasKey(e => e.QrCodeShellId);

            builder.Property(e => e.QrCodeShellId).HasColumnType("bigint").ValueGeneratedNever();
             

            builder.Property(e => e.StyleCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.ShellAlias).HasMaxLength(20).IsRequired();
            builder.Property(e => e.ConfigInfo).HasMaxLength(4096).IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BelongInfoId).HasMaxLength(64).IsRequired();

            builder.Property(e => e.CreatedOnUtc) .HasColumnType("datetime2(6)").HasDefaultValueSql("GETUTCDATE()");
            
        }
    }
}

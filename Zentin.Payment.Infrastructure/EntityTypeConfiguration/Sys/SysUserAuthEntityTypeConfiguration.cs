using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysUserAuthEntityTypeConfiguration : IEntityTypeConfiguration<SysUserAuth>
    {
        public void Configure(EntityTypeBuilder<SysUserAuth> builder)
        {
            builder.ToTable("SysUserAuth");
            builder.HasKey(e => e.AuthId);

            builder.Property(e => e.AuthId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.UserId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.IdentityType).HasConversion<int>().IsRequired();
            builder.Property(e => e.Identifier).HasMaxLength(128).IsRequired();
            builder.Property(e => e.Credential).HasMaxLength(128).IsRequired();
            builder.Property(e => e.Salt).HasMaxLength(128).IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();

            builder.HasIndex(e => new { e.SysType, e.IdentityType, e.Identifier })
                .HasDatabaseName("UniSysTypeIdentityTypeIdentifier").IsUnique();
            builder.HasIndex(e => e.UserId).HasDatabaseName("IdxUserId");
        }
    }
}
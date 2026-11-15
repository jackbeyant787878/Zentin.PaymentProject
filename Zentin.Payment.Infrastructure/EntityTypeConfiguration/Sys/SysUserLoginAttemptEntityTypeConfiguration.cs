using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysUserLoginAttemptEntityTypeConfiguration : IEntityTypeConfiguration<SysUserLoginAttempt>
    {
        public void Configure(EntityTypeBuilder<SysUserLoginAttempt> builder)
        {
            builder.ToTable("SysUserLoginAttempt");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.UserId).HasColumnType("bigint").IsRequired();
            builder.Property(e => e.IdentityType).HasConversion<int>().HasDefaultValue(IdentityType.Nickname);
            builder.Property(e => e.Identifier).HasMaxLength(128).IsRequired();
            builder.Property(e => e.IpAddress).HasMaxLength(128).IsRequired();
            builder.Property(e => e.Success).IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.AttemptTime).HasColumnType("datetime2(6)");

            builder.HasIndex(e => new { e.SysType, e.Identifier, e.AttemptTime })
                .HasDatabaseName("IdxSysTypeIdentifierAttemptTime");
        }
    }
}
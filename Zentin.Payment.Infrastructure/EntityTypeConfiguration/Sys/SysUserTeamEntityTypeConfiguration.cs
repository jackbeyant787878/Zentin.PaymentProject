using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysUserTeamEntityTypeConfiguration : IEntityTypeConfiguration<SysUserTeam>
    {
        public void Configure(EntityTypeBuilder<SysUserTeam> builder)
        {
            builder.ToTable("SysUserTeam");
            builder.HasKey(e => e.TeamId);

            builder.Property(e => e.TeamId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.TeamName).HasMaxLength(32).IsRequired();
            builder.Property(e => e.TeamNo).HasMaxLength(64).IsRequired();
            builder.Property(e => e.StatRangeType).HasMaxLength(20).IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BelongInfoId).HasMaxLength(64).HasDefaultValue("0").IsRequired();
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

            builder.HasIndex(e => e.TeamNo).HasDatabaseName("UniTeamNo").IsUnique();
            builder.HasIndex(e => new { e.SysType, e.BelongInfoId }).HasDatabaseName("IdxSysTypeBelongInfoId");
        }
    }
}
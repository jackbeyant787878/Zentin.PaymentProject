using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysUserEntityTypeConfiguration : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            builder.ToTable("SysUser");
            builder.HasKey(e => e.SysUserId);

            builder.Property(e => e.SysUserId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.LoginUsername).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Realname).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Telphone).HasMaxLength(32).IsRequired();
            builder.Property(e => e.Sex).HasConversion<int>().HasDefaultValue(Sex.Male);
            builder.Property(e => e.AvatarUrl).HasMaxLength(128);
            builder.Property(e => e.UserNo).HasMaxLength(32);
            builder.Property(e => e.SafeWord).HasMaxLength(32);
            builder.Property(e => e.InitUser).HasDefaultValue(false);
            builder.Property(e => e.UserType).HasConversion<int>().IsRequired();
            builder.Property(e => e.EntRules).HasColumnType("nvarchar(500)");
            builder.Property(e => e.BindStoreIds).HasColumnType("nvarchar(500)");
            builder.Property(e => e.InviteCode).HasMaxLength(20);
            builder.Property(e => e.TeamId).HasColumnType("bigint");
            builder.Property(e => e.IsTeamLeader).HasColumnType("tinyint");
            builder.Property(e => e.State).HasConversion<int>().HasDefaultValue(SysUserState.Normal);
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.BelongInfoId).HasMaxLength(64).HasDefaultValue("0").IsRequired();

            builder.HasIndex(e => new { e.SysType, e.LoginUsername }).HasDatabaseName("UniSysTypeLoginUsername").IsUnique();
            builder.HasIndex(e => new { e.SysType, e.Telphone }).HasDatabaseName("UniSysTypeTelphone").IsUnique();
            builder.HasIndex(e => new { e.SysType, e.UserNo }).HasDatabaseName("UniSysTypeUserNo").IsUnique()
                .HasFilter("[UserNo] IS NOT NULL");
            builder.HasIndex(e => e.InviteCode).HasDatabaseName("UniInviteCode").IsUnique()
                .HasFilter("[InviteCode] IS NOT NULL");
            builder.HasIndex(e => e.TeamId).HasDatabaseName("IdxTeamId");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysLogEntityTypeConfiguration : IEntityTypeConfiguration<SysLog>
    {
        public void Configure(EntityTypeBuilder<SysLog> builder)
        {
            builder.ToTable("SysLog");
            builder.HasKey(e => e.SysLogId);

            builder.Property(e => e.SysLogId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.UserId).HasColumnType("bigint");
            builder.Property(e => e.UserName).HasMaxLength(32);
            builder.Property(e => e.Browser).HasMaxLength(60).IsRequired();
            builder.Property(e => e.Os).HasMaxLength(60).IsRequired();
            builder.Property(e => e.Device).HasMaxLength(60).IsRequired();
            builder.Property(e => e.BrowserInfo).HasMaxLength(200).IsRequired();
            builder.Property(e => e.UserIp).HasMaxLength(128).HasDefaultValue("").IsRequired();
            builder.Property(e => e.SysType).HasMaxLength(8).IsRequired();
            builder.Property(e => e.MethodName).HasMaxLength(128).HasDefaultValue("").IsRequired();
            builder.Property(e => e.LogType).HasColumnType("tinyint");
            builder.Property(e => e.MethodRemark).HasMaxLength(128).HasDefaultValue("").IsRequired();
            builder.Property(e => e.ReqUrl).HasMaxLength(256).HasDefaultValue("").IsRequired();
            builder.Property(e => e.ReqMethod).HasMaxLength(10).IsRequired();
            builder.Property(e => e.OptReqParam).HasMaxLength(2048).HasDefaultValue("").IsRequired();
            builder.Property(e => e.OptResInfo).HasMaxLength(2048).HasDefaultValue("").IsRequired();
            builder.Property(e => e.ElapsedMs).HasColumnType("bigint");

            builder.HasIndex(e => e.CreatedOnUtc).HasDatabaseName("IdxCreatedOnUtc");
            builder.HasIndex(e => new { e.SysType, e.UserId }).HasDatabaseName("IdxSysTypeUserId");
        }
    }
}
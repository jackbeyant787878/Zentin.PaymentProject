using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Sys
{
    public sealed class SysArticleEntityTypeConfiguration : IEntityTypeConfiguration<SysArticle>
    {
        public void Configure(EntityTypeBuilder<SysArticle> builder)
        {
            builder.ToTable("SysArticle");
            builder.HasKey(e => e.ArticleId);

            builder.Property(e => e.ArticleId).HasColumnType("bigint").ValueGeneratedNever();
            builder.Property(e => e.Title).HasMaxLength(64).IsRequired();
            builder.Property(e => e.Subtitle).HasMaxLength(64).IsRequired();
            builder.Property(e => e.ArticleType).HasConversion<int>().HasDefaultValue(ArticleType.Announcement);
            builder.Property(e => e.ArticleRange).HasColumnType("nvarchar(500)").IsRequired();
            builder.Property(e => e.Content).HasColumnType("nvarchar(max)");
            builder.Property(e => e.Publisher).HasMaxLength(32).IsRequired();
            builder.Property(e => e.PublishTime).HasColumnType("datetime2(6)");
            builder.Property(e => e.CreatedUid).HasColumnType("bigint");
            builder.Property(e => e.CreatedBy).HasMaxLength(64);

            builder.HasIndex(e => e.PublishTime).HasDatabaseName("IdxPublishTime");
        }
    }
}
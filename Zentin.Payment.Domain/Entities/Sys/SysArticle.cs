using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysArticle : AggregateRoot
    {
        public long ArticleId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Subtitle { get; private set; } = string.Empty;
        public ArticleType ArticleType { get; private set; }
        public string ArticleRange { get; private set; } = string.Empty;
        public string? Content { get; private set; }
        public string Publisher { get; private set; } = string.Empty;
        public DateTime? PublishTime { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        private SysArticle()
        {
            ArticleId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static SysArticle Create(
            string title,
            string subtitle,
            ArticleType articleType,
            string articleRange,
            string publisher,
            string? content = null,
            long? createdUid = null,
            string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Article title is required", nameof(title));
            if (string.IsNullOrWhiteSpace(articleRange))
                throw new ArgumentException("Article range is required", nameof(articleRange));

            return new SysArticle
            {
                Title = title,
                Subtitle = subtitle,
                ArticleType = articleType,
                ArticleRange = articleRange,
                Content = content,
                Publisher = publisher,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };
        }

        public void Publish()
        {
            if (PublishTime.HasValue)
                throw new InvalidOperationException("Article is already published");

            PublishTime = DateTime.UtcNow;
        }

        public void UpdateContent(string title, string subtitle, string? content)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Article title is required", nameof(title));

            Title = title;
            Subtitle = subtitle;
            Content = content;
        }
    }
}
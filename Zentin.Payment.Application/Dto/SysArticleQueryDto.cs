

using Zentin.Payment.Application.Contracts.Queries.Abstractions;

namespace Zentin.Payment.Application.Dto
{
    /// <summary>
    /// 文章信息表
    /// </summary>
    public class SysArticleQueryDto : DatePagedQuery
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public long? ArticleId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章类型: 1-公告
        /// </summary>
        public byte? ArticleType { get; set; }

        /// <summary>
        /// 文章范围
        /// </summary>
        public string ArticleRange { get; set; }
    }
}

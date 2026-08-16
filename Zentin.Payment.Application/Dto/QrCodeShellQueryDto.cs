

using Zentin.Payment.Application.Contracts.Queries.Abstractions;

namespace Zentin.Payment.Application.Dto
{
    /// <summary>
    /// 码牌模板信息表
    /// </summary>
    public class QrCodeShellQueryDto : PagedQuery
    {
        /// <summary>
        /// 模板别名
        /// </summary>
        public string ShellAlias { get; set; }
    }
}

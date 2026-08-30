

using Zentin.Payment.Application.Contracts.Queries.Abstractions;

namespace Zentin.Payment.Application.Dto
{
    /// <summary>
    /// 系统角色权限关联表 system role entitlement relate
    /// </summary>
    public class SysRoleEntRelaQueryDto : PagedQuery
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }
    }
}

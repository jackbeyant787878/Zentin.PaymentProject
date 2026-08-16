namespace Zentin.Payment.Application.Dto
{
    /// <summary>
    /// 操作员<->角色 关联表
    /// </summary>
    public class SysUserRoleRelaDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }
    }
}

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysUserRoleRela
    {
        public long UserId { get; private set; }
        public string RoleId { get; private set; } = string.Empty;

        private SysUserRoleRela() { }

        public static SysUserRoleRela Create(long userId, string roleId)
        {
            if (userId <= 0)
                throw new ArgumentException("User id is required", nameof(userId));
            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentException("Role id is required", nameof(roleId));

            return new SysUserRoleRela { UserId = userId, RoleId = roleId };
        }
    }
}
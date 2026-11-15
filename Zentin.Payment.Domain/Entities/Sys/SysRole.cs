using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysRole : AggregateRoot
    {
        public string RoleId { get; private set; } = string.Empty;
        public string RoleName { get; private set; } = string.Empty;
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;

        private SysRole() { }

        public static SysRole Create(string roleId, string roleName, string sysType, string belongInfoId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentException("Role id is required", nameof(roleId));
            if (!roleId.StartsWith("ROLE_", StringComparison.Ordinal))
                throw new ArgumentException("Role id must start with ROLE_", nameof(roleId));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name is required", nameof(roleName));

            return new SysRole
            {
                RoleId = roleId,
                RoleName = roleName,
                SysType = sysType,
                BelongInfoId = belongInfoId
            };
        }

        public void Rename(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name is required", nameof(roleName));

            RoleName = roleName;
        }
    }
}
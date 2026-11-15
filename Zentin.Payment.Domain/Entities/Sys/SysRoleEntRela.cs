namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysRoleEntRela
    {
        public string RoleId { get; private set; } = string.Empty;
        public string EntId { get; private set; } = string.Empty;

        private SysRoleEntRela() { }

        public static SysRoleEntRela Create(string roleId, string entId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentException("Role id is required", nameof(roleId));
            if (string.IsNullOrWhiteSpace(entId))
                throw new ArgumentException("Entitlement id is required", nameof(entId));

            return new SysRoleEntRela { RoleId = roleId, EntId = entId };
        }
    }
}
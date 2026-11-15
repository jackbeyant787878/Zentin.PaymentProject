using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysUserRoleRelaRepository
    {
        Task<IReadOnlyList<SysUserRoleRela>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysUserRoleRela>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(long userId, string roleId, CancellationToken cancellationToken = default);

        void AddRange(IEnumerable<SysUserRoleRela> relations);

        void RemoveRange(IEnumerable<SysUserRoleRela> relations);

        Task RemoveByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    }
}
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysRoleEntRelaRepository
    {
        Task<IReadOnlyList<SysRoleEntRela>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysRoleEntRela>> GetByRoleIdsAsync(IEnumerable<string> roleIds, CancellationToken cancellationToken = default);

        void AddRange(IEnumerable<SysRoleEntRela> relations);

        void RemoveRange(IEnumerable<SysRoleEntRela> relations);

        Task RemoveByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);
    }
}
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysRoleRepository : IRepository<SysRole, string>
    {
        Task<IReadOnlyList<SysRole>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysRole>> GetByIdsAsync(IEnumerable<string> roleIds, CancellationToken cancellationToken = default);
    }
}
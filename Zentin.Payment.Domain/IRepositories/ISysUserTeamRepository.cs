using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysUserTeamRepository : IRepository<SysUserTeam, long>
    {
        Task<SysUserTeam?> GetByTeamNoAsync(string teamNo, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysUserTeam>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default);
    }
}
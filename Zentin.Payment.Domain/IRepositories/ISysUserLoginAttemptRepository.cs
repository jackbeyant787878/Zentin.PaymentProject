using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysUserLoginAttemptRepository : IRepository<SysUserLoginAttempt, long>
    {
        Task<int> CountFailuresSinceAsync(string sysType, string identifier, DateTime sinceUtc, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysUserLoginAttempt>> GetRecentAsync(string sysType, string identifier, int take, CancellationToken cancellationToken = default);
    }
}
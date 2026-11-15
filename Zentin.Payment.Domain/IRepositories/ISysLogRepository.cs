using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysLogRepository : IRepository<SysLog, long>
    {
        Task<IReadOnlyList<SysLog>> GetByUserAsync(string sysType, long userId, int take, CancellationToken cancellationToken = default);
    }
}
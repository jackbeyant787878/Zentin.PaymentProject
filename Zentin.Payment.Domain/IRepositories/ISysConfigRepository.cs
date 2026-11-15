using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysConfigRepository
    {
        Task<SysConfig?> GetAsync(string configKey, string sysType, string belongInfoId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysConfig>> GetByGroupAsync(string sysType, string belongInfoId, string groupKey, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysConfig>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default);

        void Add(SysConfig config);

        void Update(SysConfig config);

        void Remove(SysConfig config);
    }
}
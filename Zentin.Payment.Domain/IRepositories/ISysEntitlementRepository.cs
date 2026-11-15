using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysEntitlementRepository
    {
        Task<SysEntitlement?> GetAsync(string entId, string sysType, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysEntitlement>> GetBySysTypeAsync(string sysType, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysEntitlement>> GetByEntIdsAsync(IEnumerable<string> entIds, string sysType, CancellationToken cancellationToken = default);

        void Add(SysEntitlement entitlement);

        void AddRange(IEnumerable<SysEntitlement> entitlements);

        void Update(SysEntitlement entitlement);

        void Remove(SysEntitlement entitlement);
    }
}
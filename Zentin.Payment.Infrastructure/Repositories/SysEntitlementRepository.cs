using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysEntitlementRepository : ISysEntitlementRepository
    {
        private readonly DbSet<SysEntitlement> _dbSet;

        public SysEntitlementRepository(PaymentDbContext dbContext)
        {
            _dbSet = dbContext.Set<SysEntitlement>();
        }

        public async Task<SysEntitlement?> GetAsync(string entId, string sysType, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object?[] { entId, sysType }, cancellationToken);
        }

        public async Task<IReadOnlyList<SysEntitlement>> GetBySysTypeAsync(string sysType, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(e => e.SysType == sysType).OrderBy(e => e.EntSort).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SysEntitlement>> GetByEntIdsAsync(IEnumerable<string> entIds, string sysType, CancellationToken cancellationToken = default)
        {
            var ids = entIds.ToArray();
            return await _dbSet.AsNoTracking().Where(e => e.SysType == sysType && ids.Contains(e.EntId)).ToListAsync(cancellationToken);
        }

        public void Add(SysEntitlement entitlement) => _dbSet.Add(entitlement);

        public void AddRange(IEnumerable<SysEntitlement> entitlements) => _dbSet.AddRange(entitlements);

        public void Update(SysEntitlement entitlement) => _dbSet.Update(entitlement);

        public void Remove(SysEntitlement entitlement) => _dbSet.Remove(entitlement);
    }
}
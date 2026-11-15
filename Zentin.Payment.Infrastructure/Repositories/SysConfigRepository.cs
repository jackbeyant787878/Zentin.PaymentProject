using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysConfigRepository : ISysConfigRepository
    {
        private readonly DbSet<SysConfig> _dbSet;

        public SysConfigRepository(PaymentDbContext dbContext)
        {
            _dbSet = dbContext.Set<SysConfig>();
        }

        public async Task<SysConfig?> GetAsync(string configKey, string sysType, string belongInfoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object?[] { configKey, sysType, belongInfoId }, cancellationToken);
        }

        public async Task<IReadOnlyList<SysConfig>> GetByGroupAsync(string sysType, string belongInfoId, string groupKey, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.SysType == sysType && c.BelongInfoId == belongInfoId && c.GroupKey == groupKey)
                .OrderBy(c => c.SortNum)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SysConfig>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.SysType == sysType && c.BelongInfoId == belongInfoId)
                .OrderBy(c => c.SortNum)
                .ToListAsync(cancellationToken);
        }

        public void Add(SysConfig config) => _dbSet.Add(config);

        public void Update(SysConfig config) => _dbSet.Update(config);

        public void Remove(SysConfig config) => _dbSet.Remove(config);
    }
}
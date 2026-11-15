using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysUserLoginAttemptRepository : RepositoryBase<SysUserLoginAttempt, long>, ISysUserLoginAttemptRepository
    {
        public SysUserLoginAttemptRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<int> CountFailuresSinceAsync(string sysType, string identifier, DateTime sinceUtc, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .CountAsync(a => a.SysType == sysType && a.Identifier == identifier && !a.Success && a.AttemptTime >= sinceUtc, cancellationToken);
        }

        public async Task<IReadOnlyList<SysUserLoginAttempt>> GetRecentAsync(string sysType, string identifier, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(a => a.SysType == sysType && a.Identifier == identifier)
                .OrderByDescending(a => a.AttemptTime)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysLogRepository : RepositoryBase<SysLog, long>, ISysLogRepository
    {
        public SysLogRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<SysLog>> GetByUserAsync(string sysType, long userId, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(l => l.SysType == sysType && l.UserId == userId)
                .OrderByDescending(l => l.CreatedOnUtc)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
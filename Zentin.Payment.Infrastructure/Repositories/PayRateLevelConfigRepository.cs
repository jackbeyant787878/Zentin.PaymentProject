using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayRateLevelConfigRepository : RepositoryBase<PayRateLevelConfig, long>, IPayRateLevelConfigRepository
    {
        public PayRateLevelConfigRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<PayRateLevelConfig>> GetByRateConfigIdAsync(long rateConfigId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking() .Where(l => l.RateConfigId == rateConfigId).ToListAsync(cancellationToken);
        }
    }
}

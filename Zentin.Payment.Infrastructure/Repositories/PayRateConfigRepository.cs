using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayRateConfigRepository : RepositoryBase<PayRateConfig, long>, IPayRateConfigRepository
    {
        public PayRateConfigRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<PayRateConfig?> GetByUniqueKeyAsync(string configType, string infoType, string infoId, string ifCode, string wayCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(r => r.LevelConfigs).FirstOrDefaultAsync(r => r.State == true && r.ConfigType == configType && r.InfoType == infoType
                    && r.InfoId == infoId && r.IfCode == ifCode && r.WayCode == wayCode, cancellationToken);
        }

        public async Task<List<PayRateConfig>> GetByInfoIdAsync(string configType, string infoType, string infoId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking() .Where(r => r.State == true && r.ConfigType == configType && r.InfoType == infoType && r.InfoId == infoId).ToListAsync(cancellationToken);
        }

        public async Task<List<PayRateConfig>> GetByInfoIdAndIfCodeAsync(string configType, string infoType, string infoId, string ifCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking() .Where(r => r.State == true && r.ConfigType == configType && r.InfoType == infoType && r.InfoId == infoId && r.IfCode == ifCode).ToListAsync(cancellationToken);
        }
    }
}

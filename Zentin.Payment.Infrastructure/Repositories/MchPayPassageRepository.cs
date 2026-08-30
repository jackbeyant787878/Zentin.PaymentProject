using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchPayPassageRepository : RepositoryBase<MchPayPassage, long>, IMchPayPassageRepository
    {
        public MchPayPassageRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<MchPayPassage>> GetByMchAppAsync(string mchNo, string appId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(p => p.MchNo == mchNo && p.AppId == appId && p.State == true).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExistUseWayCodeAsync(string wayCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(p => p.WayCode == wayCode, cancellationToken);
        }
    }
}

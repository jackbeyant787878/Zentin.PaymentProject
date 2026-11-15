using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MbrRechargeOrderRepository : RepositoryBase<MbrRechargeOrder, string>, IMbrRechargeOrderRepository
    {
        public MbrRechargeOrderRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MbrRechargeOrder>> GetByMbrNoAsync(string mbrNo, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(o => o.MbrNo == mbrNo)
                .OrderByDescending(o => o.CreatedOnUtc)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<MbrRechargeOrder>> GetByMchNoAsync(string mchNo, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(o => o.MchNo == mchNo)
                .OrderByDescending(o => o.CreatedOnUtc)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
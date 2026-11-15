using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MbrBillRepository : RepositoryBase<MbrBill, string>, IMbrBillRepository
    {
        public MbrBillRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MbrBill>> GetByMbrNoAsync(string mbrNo, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(b => b.MbrNo == mbrNo)
                .OrderByDescending(b => b.CreatedOnUtc)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchStoreRepository : RepositoryBase<MchStore, long>, IMchStoreRepository
    {
        public MchStoreRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MchStore>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(s => s.MchNo == mchNo).ToListAsync(cancellationToken);
        }

        public async Task<MchStore?> GetDefaultStoreAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(s => s.MchNo == mchNo && s.DefaultFlag == 1, cancellationToken);
        }

        public async Task<IReadOnlyList<MchStore>> GetByBindAppIdAsync(string bindAppId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(s => s.BindAppId == bindAppId).ToListAsync(cancellationToken);
        }
    }
}
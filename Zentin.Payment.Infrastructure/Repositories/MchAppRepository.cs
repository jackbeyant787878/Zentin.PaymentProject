using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchAppRepository : RepositoryBase<MchApp, string>, IMchAppRepository
    {
        public MchAppRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MchApp>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(a => a.MchNo == mchNo).ToListAsync(cancellationToken);
        }

        public async Task<MchApp?> GetDefaultAppAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(a => a.MchNo == mchNo && a.DefaultFlag == 1, cancellationToken);
        }

        public async Task<bool> ExistsByAppIdAsync(string appId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(a => a.AppId == appId, cancellationToken);
        }
    }
}
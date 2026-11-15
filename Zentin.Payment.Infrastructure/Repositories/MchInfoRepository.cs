using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchInfoRepository : RepositoryBase<MchInfo, string>, IMchInfoRepository
    {
        public MchInfoRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<MchInfo?> GetWithAppsAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(m => m.Apps).FirstOrDefaultAsync(m => m.MchNo == mchNo, cancellationToken);
        }

        public async Task<MchInfo?> GetWithStoresAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(m => m.Stores).FirstOrDefaultAsync(m => m.MchNo == mchNo, cancellationToken);
        }

        public async Task<MchInfo?> GetWithRechargeRulesAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(m => m.RechargeRules).FirstOrDefaultAsync(m => m.MchNo == mchNo, cancellationToken);
        }

        public async Task<IReadOnlyList<MchInfo>> GetByAgentNoAsync(string agentNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(m => m.AgentNo == agentNo).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(m => m.MchNo == mchNo, cancellationToken);
        }
    }
}
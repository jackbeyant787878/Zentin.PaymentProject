using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MbrRechargeRuleRepository : RepositoryBase<MbrRechargeRule, long>, IMbrRechargeRuleRepository
    {
        public MbrRechargeRuleRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MbrRechargeRule>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(r => r.MchNo == mchNo).OrderBy(r => r.Sort).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<MbrRechargeRule>> GetEnabledByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(r => r.MchNo == mchNo && r.State == MbrState.Normal)
                .OrderBy(r => r.Sort)
                .ToListAsync(cancellationToken);
        }
    }
}
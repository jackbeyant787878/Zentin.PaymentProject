using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchApplyRepository : RepositoryBase<MchApply, string>, IMchApplyRepository
    {
        public MchApplyRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MchApply>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(a => a.MchNo == mchNo).ToListAsync(cancellationToken);
        }

        public async Task<MchApply?> GetByChannelApplyNoAsync(string ifCode, string channelApplyNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(a => a.IfCode == ifCode && a.ChannelApplyNo == channelApplyNo, cancellationToken);
        }

        public async Task<IReadOnlyList<MchApply>> GetByStateAsync(MchApplyState state, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(a => a.State == state).OrderBy(a => a.LastApplyAt).Take(take).ToListAsync(cancellationToken);
        }
    }
}
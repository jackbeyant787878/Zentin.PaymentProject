using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Member;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MbrInfoRepository : RepositoryBase<MbrInfo, string>, IMbrInfoRepository
    {
        public MbrInfoRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<MbrInfo>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(m => m.MchNo == mchNo).ToListAsync(cancellationToken);
        }

        public async Task<MbrInfo?> GetByTelAsync(string mchNo, string mbrTel, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(m => m.MchNo == mchNo && m.MbrTel == mbrTel, cancellationToken);
        }

        public async Task<MbrInfo?> GetByWxOpenIdAsync(string mchNo, string wxMpOpenId, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(m => m.MchNo == mchNo && m.WxMpOpenId == wxMpOpenId, cancellationToken);
        }

        public async Task<MbrInfo?> GetByAliUserIdAsync(string mchNo, string aliUserId, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(m => m.MchNo == mchNo && m.AliUserId == aliUserId, cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysUserTeamRepository : RepositoryBase<SysUserTeam, long>, ISysUserTeamRepository
    {
        public SysUserTeamRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<SysUserTeam?> GetByTeamNoAsync(string teamNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.TeamNo == teamNo, cancellationToken);
        }

        public async Task<IReadOnlyList<SysUserTeam>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(t => t.SysType == sysType && t.BelongInfoId == belongInfoId).ToListAsync(cancellationToken);
        }
    }
}
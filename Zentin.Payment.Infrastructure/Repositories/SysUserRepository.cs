using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysUserRepository : RepositoryBase<SysUser, long>, ISysUserRepository
    {
        public SysUserRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<SysUser?> GetByUsernameAsync(string sysType, string loginUsername, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.SysType == sysType && u.LoginUsername == loginUsername, cancellationToken);
        }

        public async Task<SysUser?> GetByTelphoneAsync(string sysType, string telphone, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.SysType == sysType && u.Telphone == telphone, cancellationToken);
        }

        public async Task<SysUser?> GetByInviteCodeAsync(string inviteCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.InviteCode == inviteCode, cancellationToken);
        }

        public async Task<bool> ExistsByUsernameAsync(string sysType, string loginUsername, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(u => u.SysType == sysType && u.LoginUsername == loginUsername, cancellationToken);
        }

        public async Task<IReadOnlyList<SysUser>> GetByTeamAsync(long teamId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(u => u.TeamId == teamId).ToListAsync(cancellationToken);
        }
    }
}
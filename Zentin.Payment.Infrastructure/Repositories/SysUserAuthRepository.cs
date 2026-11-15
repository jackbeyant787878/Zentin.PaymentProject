using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysUserAuthRepository : RepositoryBase<SysUserAuth, long>, ISysUserAuthRepository
    {
        public SysUserAuthRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<SysUserAuth?> GetAsync(string sysType, IdentityType identityType, string identifier, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(a => a.SysType == sysType && a.IdentityType == identityType && a.Identifier == identifier, cancellationToken);
        }

        public async Task<IReadOnlyList<SysUserAuth>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(a => a.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(string sysType, IdentityType identityType, string identifier, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(a => a.SysType == sysType && a.IdentityType == identityType && a.Identifier == identifier, cancellationToken);
        }
    }
}
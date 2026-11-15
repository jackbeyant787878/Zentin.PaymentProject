using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysRoleRepository : RepositoryBase<SysRole, string>, ISysRoleRepository
    {
        public SysRoleRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<SysRole>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(r => r.SysType == sysType && r.BelongInfoId == belongInfoId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SysRole>> GetByIdsAsync(IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
        {
            var ids = roleIds.ToArray();
            return await DbSet.AsNoTracking().Where(r => ids.Contains(r.RoleId)).ToListAsync(cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysRoleEntRelaRepository : ISysRoleEntRelaRepository
    {
        private readonly DbSet<SysRoleEntRela> _dbSet;

        public SysRoleEntRelaRepository(PaymentDbContext dbContext)
        {
            _dbSet = dbContext.Set<SysRoleEntRela>();
        }

        public async Task<IReadOnlyList<SysRoleEntRela>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(r => r.RoleId == roleId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SysRoleEntRela>> GetByRoleIdsAsync(IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
        {
            var ids = roleIds.ToArray();
            return await _dbSet.AsNoTracking().Where(r => ids.Contains(r.RoleId)).ToListAsync(cancellationToken);
        }

        public void AddRange(IEnumerable<SysRoleEntRela> relations) => _dbSet.AddRange(relations);

        public void RemoveRange(IEnumerable<SysRoleEntRela> relations) => _dbSet.RemoveRange(relations);

        public async Task RemoveByRoleIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            await _dbSet.Where(r => r.RoleId == roleId).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysUserRoleRelaRepository : ISysUserRoleRelaRepository
    {
        private readonly DbSet<SysUserRoleRela> _dbSet;

        public SysUserRoleRelaRepository(PaymentDbContext dbContext)
        {
            _dbSet = dbContext.Set<SysUserRoleRela>();
        }

        public async Task<IReadOnlyList<SysUserRoleRela>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(r => r.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SysUserRoleRela>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(r => r.RoleId == roleId).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(long userId, string roleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(r => r.UserId == userId && r.RoleId == roleId, cancellationToken);
        }

        public void AddRange(IEnumerable<SysUserRoleRela> relations) => _dbSet.AddRange(relations);

        public void RemoveRange(IEnumerable<SysUserRoleRela> relations) => _dbSet.RemoveRange(relations);

        public async Task RemoveByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            await _dbSet.Where(r => r.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
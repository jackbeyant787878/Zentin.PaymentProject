using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysUserAuthRepository : IRepository<SysUserAuth, long>
    {
        Task<SysUserAuth?> GetAsync(string sysType, IdentityType identityType, string identifier, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysUserAuth>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(string sysType, IdentityType identityType, string identifier, CancellationToken cancellationToken = default);
    }
}
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysUserRepository : IRepository<SysUser, long>
    {
        Task<SysUser?> GetByUsernameAsync(string sysType, string loginUsername, CancellationToken cancellationToken = default);

        Task<SysUser?> GetByTelphoneAsync(string sysType, string telphone, CancellationToken cancellationToken = default);

        Task<SysUser?> GetByInviteCodeAsync(string inviteCode, CancellationToken cancellationToken = default);

        Task<bool> ExistsByUsernameAsync(string sysType, string loginUsername, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SysUser>> GetByTeamAsync(long teamId, CancellationToken cancellationToken = default);
    }
}
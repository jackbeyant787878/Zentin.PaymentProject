using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Merchant;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchAppRepository : IRepository<MchApp, string>
    {
        Task<IReadOnlyList<MchApp>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MchApp?> GetDefaultAppAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MchApp?> GetByMchNoAndAppIdAsync(string mchNo, string appId, CancellationToken cancellationToken = default);

        Task<bool> ExistsByAppIdAsync(string appId, CancellationToken cancellationToken = default);
    }
}
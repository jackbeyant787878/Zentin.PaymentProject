using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Merchant;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchStoreRepository : IRepository<MchStore, long>
    {
        Task<IReadOnlyList<MchStore>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MchStore?> GetDefaultStoreAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MchStore>> GetByBindAppIdAsync(string bindAppId, CancellationToken cancellationToken = default);
    }
}
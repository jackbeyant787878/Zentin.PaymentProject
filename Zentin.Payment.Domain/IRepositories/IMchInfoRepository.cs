using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Merchant;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchInfoRepository : IRepository<MchInfo, string>
    {
        Task<MchInfo?> GetWithAppsAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MchInfo?> GetWithStoresAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MchInfo?> GetWithRechargeRulesAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MchInfo>> GetByAgentNoAsync(string agentNo, CancellationToken cancellationToken = default);

        Task<bool> ExistsByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);
    }
}
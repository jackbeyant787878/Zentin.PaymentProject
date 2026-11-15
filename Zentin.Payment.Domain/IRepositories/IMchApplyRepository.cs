using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Merchant;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchApplyRepository : IRepository<MchApply, string>
    {
        Task<IReadOnlyList<MchApply>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MchApply?> GetByChannelApplyNoAsync(string ifCode, string channelApplyNo, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MchApply>> GetByStateAsync(MchApplyState state, int take, CancellationToken cancellationToken = default);
    }
}
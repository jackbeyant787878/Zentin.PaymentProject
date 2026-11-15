using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Member;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMbrRechargeOrderRepository : IRepository<MbrRechargeOrder, string>
    {
        Task<IReadOnlyList<MbrRechargeOrder>> GetByMbrNoAsync(string mbrNo, int take, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MbrRechargeOrder>> GetByMchNoAsync(string mchNo, int take, CancellationToken cancellationToken = default);
    }
}
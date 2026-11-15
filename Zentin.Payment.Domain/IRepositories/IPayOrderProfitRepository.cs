using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayOrderProfitRepository : IRepository<PayOrderProfit, long>
    {
        Task<IReadOnlyList<PayOrderProfit>> GetByPayOrderIdAsync(string payOrderId, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(string infoId, string infoType, string payOrderId, CancellationToken cancellationToken = default);
    }
}
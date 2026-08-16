using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Order;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IRefundOrderRepository : IRepository<RefundOrder, string>
    {
        Task<RefundOrder?> GetByMchRefundNoAsync(string mchNo, string mchRefundNo, CancellationToken cancellationToken = default);

        Task<bool> IsExistByMchRefundNoAsync(string mchNo, string mchRefundNo, CancellationToken cancellationToken = default);

        Task<bool> HasRefundingOrderAsync(string payOrderId, CancellationToken cancellationToken = default);
    }
}

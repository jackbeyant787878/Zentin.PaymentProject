using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Order;

namespace Zentin.Payment.Domain.IRepositories
{

    public interface IPayOrderRepository : IRepository<PayOrder, string>
    {
        Task<PayOrder?> GetByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default);

        Task<PayOrder?> GetWithRefundsAsync(string payOrderId, CancellationToken cancellationToken = default);

        Task<PayOrder?> GetWithDivisionRecordsAsync(string payOrderId, CancellationToken cancellationToken = default);

        Task<bool> IsExistByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default);

        Task<bool> IsExistUseIfCodeAsync(string ifCode, CancellationToken cancellationToken = default);

        Task<bool> IsExistUseWayCodeAsync(string wayCode, CancellationToken cancellationToken = default);

        Task<bool> IsExistUseMchNoAsync(string mchNo, CancellationToken cancellationToken = default);
    }
}

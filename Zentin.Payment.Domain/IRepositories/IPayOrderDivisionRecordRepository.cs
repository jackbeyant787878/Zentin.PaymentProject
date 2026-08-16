using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayOrderDivisionRecordRepository : IRepository<PayOrderDivisionRecord, long>
    {
        Task<List<PayOrderDivisionRecord>> GetByPayOrderIdAsync(string payOrderId, CancellationToken cancellationToken = default);

        Task<long> SumSuccessDivisionAmountAsync(string payOrderId, CancellationToken cancellationToken = default);
    }
}

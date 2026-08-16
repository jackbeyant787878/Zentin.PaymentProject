using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Notify;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchNotifyRecordRepository : IRepository<MchNotifyRecord, long>
    {
        Task<MchNotifyRecord?> GetByOrderAsync(string orderId, int orderType, CancellationToken cancellationToken = default);
    }
}

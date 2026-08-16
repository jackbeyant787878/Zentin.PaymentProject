using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.CheckBill;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IChannelBillRepository : IRepository<ChannelBill, long>
    {
        Task<List<ChannelBill>> GetByBatchNoAsync(string batchNo, CancellationToken cancellationToken = default);
    }
}

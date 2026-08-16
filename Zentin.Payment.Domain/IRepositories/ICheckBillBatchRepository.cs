using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.CheckBill;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ICheckBillBatchRepository : IRepository<CheckBillBatch, string>
    {
        Task<CheckBillBatch?> GetByDateAsync(string ifCode, DateTime billDate, string? channelMchNo = null, CancellationToken cancellationToken = default);
    }
}

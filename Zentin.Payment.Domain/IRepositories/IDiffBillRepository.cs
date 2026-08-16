using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.CheckBill;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IDiffBillRepository : IRepository<DiffBill, long>
    {
        Task<List<DiffBill>> GetUnhandledByBatchNoAsync(string batchNo, CancellationToken cancellationToken = default);
    }
}

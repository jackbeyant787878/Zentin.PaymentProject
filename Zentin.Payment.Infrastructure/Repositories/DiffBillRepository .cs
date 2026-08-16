using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.CheckBill;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class DiffBillRepository : RepositoryBase<DiffBill, long>, IDiffBillRepository
    {
        public DiffBillRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<DiffBill>> GetUnhandledByBatchNoAsync(string batchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(b => b.BatchNo == batchNo && b.HandleState == DiffBillHandleState.Unhandled).ToListAsync(cancellationToken);
        }
    }
}

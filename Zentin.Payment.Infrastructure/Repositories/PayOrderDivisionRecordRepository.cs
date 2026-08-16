using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayOrderDivisionRecordRepository : RepositoryBase<PayOrderDivisionRecord, long>, IPayOrderDivisionRecordRepository
    {
        public PayOrderDivisionRecordRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<PayOrderDivisionRecord>> GetByPayOrderIdAsync(string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(r => r.PayOrderId == payOrderId).ToListAsync(cancellationToken);
        }

        public async Task<long> SumSuccessDivisionAmountAsync(string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(r => r.PayOrderId == payOrderId && r.State == DivisionRecordState.Success).SumAsync(r => r.CalDivisionAmount, cancellationToken);
        }
    }
}

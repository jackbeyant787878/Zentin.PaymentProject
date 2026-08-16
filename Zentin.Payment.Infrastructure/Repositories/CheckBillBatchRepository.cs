using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.CheckBill;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class CheckBillBatchRepository : RepositoryBase<CheckBillBatch, string>, ICheckBillBatchRepository
    {
        public CheckBillBatchRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<CheckBillBatch?> GetByDateAsync(string ifCode, DateTime billDate, string? channelMchNo = null, CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsNoTracking().Where(b => b.IfCode == ifCode && b.BillDate.Date == billDate.Date);

            if (!string.IsNullOrEmpty(channelMchNo))
                query = query.Where(b => b.ChannelMchNo == channelMchNo);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}

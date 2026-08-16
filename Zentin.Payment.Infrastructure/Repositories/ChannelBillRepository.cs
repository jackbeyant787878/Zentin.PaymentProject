using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.CheckBill;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class ChannelBillRepository : RepositoryBase<ChannelBill, long>, IChannelBillRepository
    {
        public ChannelBillRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<ChannelBill>> GetByBatchNoAsync(string batchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(b => b.BatchNo == batchNo).ToListAsync(cancellationToken);
        }
    }
}

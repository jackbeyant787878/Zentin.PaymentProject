using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;
using Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class TransferOrderRepository : RepositoryBase<TransferOrder, string>, ITransferOrderRepository
    {
        public TransferOrderRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<TransferOrder?> GetByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.MchNo == mchNo && t.MchOrderNo == mchOrderNo, cancellationToken);
        }

        public async Task<bool> IsExistByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(t => t.MchNo == mchNo && t.MchOrderNo == mchOrderNo, cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.QrCode;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class QrCodeRepository : RepositoryBase<QrCode, string>, IQrCodeRepository
    {
        public QrCodeRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<bool> IsExistBatchIdAsync(string batchId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(q => q.BatchId == batchId, cancellationToken);
        }

        public async Task<List<QrCode>> GetByStoreIdAsync(long storeId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(q => q.StoreId == storeId).ToListAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.QrCode;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class QrCodeShellRepository : RepositoryBase<QrCodeShell, int>, IQrCodeShellRepository
    {
        public QrCodeShellRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<QrCodeShell>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(s => s.SysType == sysType && s.BelongInfoId == belongInfoId).ToListAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayInterfaceConfigRepository : RepositoryBase<PayInterfaceConfig, long>, IPayInterfaceConfigRepository
    {
        public PayInterfaceConfigRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<PayInterfaceConfig?> GetByInfoAsync(string infoType, string infoId, string ifCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(c =>c.InfoType == infoType && c.InfoId == infoId && c.IfCode == ifCode, cancellationToken);
        }

        public async Task<bool> IsExistUseIfCodeAsync(string ifCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(c => c.IfCode == ifCode, cancellationToken);
        }

        public async Task<bool> MchAppHasAvailableIfCodeAsync(string appId, string ifCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(c =>c.IfCode == ifCode && c.InfoId == appId && c.State == true && c.InfoType == "MCH_APP", cancellationToken);
        }
    }
}

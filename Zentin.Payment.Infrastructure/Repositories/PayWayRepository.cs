using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayWayRepository : RepositoryBase<PayWay, string>, IPayWayRepository
    {
        public PayWayRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<bool> IsExistWayCodeAsync(string wayCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(w => w.WayCode == wayCode.ToUpper(), cancellationToken);
        }
    }
}

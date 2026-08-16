using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Division;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchDivisionReceiverGroupRepository : RepositoryBase<MchDivisionReceiverGroup, long>, IMchDivisionReceiverGroupRepository
    {
        public MchDivisionReceiverGroupRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<MchDivisionReceiverGroup>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(g => g.MchNo == mchNo).ToListAsync(cancellationToken);
        }
    }
}

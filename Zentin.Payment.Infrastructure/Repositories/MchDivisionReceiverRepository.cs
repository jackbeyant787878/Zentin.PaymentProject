using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Division;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchDivisionReceiverRepository : RepositoryBase<MchDivisionReceiver, long>, IMchDivisionReceiverRepository
    {
        public MchDivisionReceiverRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<MchDivisionReceiver>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(r => r.ReceiverGroupId == groupId).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExistUseReceiverGroupAsync(long receiverGroupId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(r => r.ReceiverGroupId == receiverGroupId, cancellationToken);
        }
    }
}

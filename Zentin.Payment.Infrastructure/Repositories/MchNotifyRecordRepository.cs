using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Notify;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class MchNotifyRecordRepository : RepositoryBase<MchNotifyRecord, long>, IMchNotifyRecordRepository
    {
        public MchNotifyRecordRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<MchNotifyRecord?> GetByOrderAsync(string orderId, int orderType, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(n => n.OrderId == orderId && (int)n.OrderType == orderType, cancellationToken);
        }
    }
}

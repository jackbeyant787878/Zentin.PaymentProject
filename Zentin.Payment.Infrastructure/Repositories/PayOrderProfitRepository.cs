using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayOrderProfitRepository : RepositoryBase<PayOrderProfit, long>, IPayOrderProfitRepository
    {
        public PayOrderProfitRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<PayOrderProfit>> GetByPayOrderIdAsync(string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(p => p.PayOrderId == payOrderId).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(string infoId, string infoType, string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .AnyAsync(p => p.InfoId == infoId && p.InfoType == infoType && p.PayOrderId == payOrderId, cancellationToken);
        }
    }
}
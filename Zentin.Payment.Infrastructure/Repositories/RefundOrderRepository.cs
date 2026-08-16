using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class RefundOrderRepository : RepositoryBase<RefundOrder, string>, IRefundOrderRepository
    {
        public RefundOrderRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<RefundOrder?> GetByMchRefundNoAsync(string mchNo, string mchRefundNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(r => r.MchNo == mchNo && r.MchRefundNo == mchRefundNo, cancellationToken);
        }

        public async Task<bool> IsExistByMchRefundNoAsync(string mchNo, string mchRefundNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(r => r.MchNo == mchNo && r.MchRefundNo == mchRefundNo, cancellationToken);
        }

        public async Task<bool> HasRefundingOrderAsync(string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(r =>
                r.PayOrderId == payOrderId && r.State == RefundOrderStatus.Processing, cancellationToken);
        }
    }
}

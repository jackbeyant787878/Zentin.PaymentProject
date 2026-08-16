using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayOrderRepository : RepositoryBase<PayOrder, string>, IPayOrderRepository
    {
        public PayOrderRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<PayOrder?> GetByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(o => o.MchNo == mchNo && o.MchOrderNo == mchOrderNo, cancellationToken);
        }

        public async Task<PayOrder?> GetWithRefundsAsync(string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(o => o.RefundOrders).FirstOrDefaultAsync(o => o.PayOrderId == payOrderId, cancellationToken);
        }

        public async Task<PayOrder?> GetWithDivisionRecordsAsync(string payOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(o => o.DivisionRecords).FirstOrDefaultAsync(o => o.PayOrderId == payOrderId, cancellationToken);
        }

        public async Task<bool> IsExistByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(o => o.MchNo == mchNo && o.MchOrderNo == mchOrderNo, cancellationToken);
        }

        public async Task<bool> IsExistUseIfCodeAsync(string ifCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(o => o.IfCode == ifCode, cancellationToken);
        }

        public async Task<bool> IsExistUseWayCodeAsync(string wayCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(o => o.WayCode == wayCode, cancellationToken);
        }

        public async Task<bool> IsExistUseMchNoAsync(string mchNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(o => o.MchNo == mchNo, cancellationToken);
        }
    }
}

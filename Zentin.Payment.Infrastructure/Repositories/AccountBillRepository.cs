using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Finance;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class AccountBillRepository : RepositoryBase<AccountBill, long>, IAccountBillRepository
    {
        public AccountBillRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<AccountBill?> GetByBillIdAsync(string billId, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(b => b.BillId == billId, cancellationToken);
        }

        public async Task<IReadOnlyList<AccountBill>> GetByAccountAsync(string infoType, string infoId, int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(b => b.InfoType == infoType && b.InfoId == infoId)
                .OrderByDescending(b => b.AccountId)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AccountBill>> GetByBizOrderAsync(RelaBizOrderType relaBizOrderType, string relaBizOrderId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(b => b.RelaBizOrderType == relaBizOrderType && b.RelaBizOrderId == relaBizOrderId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByBillIdAsync(string billId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(b => b.BillId == billId, cancellationToken);
        }
    }
}
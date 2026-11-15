using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Finance;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IAccountBillRepository : IRepository<AccountBill, long>
    {
        Task<AccountBill?> GetByBillIdAsync(string billId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<AccountBill>> GetByAccountAsync(string infoType, string infoId, int take, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<AccountBill>> GetByBizOrderAsync(RelaBizOrderType relaBizOrderType, string relaBizOrderId, CancellationToken cancellationToken = default);

        Task<bool> ExistsByBillIdAsync(string billId, CancellationToken cancellationToken = default);
    }
}
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ITransferOrderRepository : IRepository<TransferOrder, string>
    {
        Task<TransferOrder?> GetByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default);

        Task<bool> IsExistByMchOrderNoAsync(string mchNo, string mchOrderNo, CancellationToken cancellationToken = default);
    }
}

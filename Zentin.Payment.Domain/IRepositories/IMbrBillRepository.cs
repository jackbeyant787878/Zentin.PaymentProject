using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Member;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMbrBillRepository : IRepository<MbrBill, string>
    {
        Task<IReadOnlyList<MbrBill>> GetByMbrNoAsync(string mbrNo, int take, CancellationToken cancellationToken = default);
    }
}
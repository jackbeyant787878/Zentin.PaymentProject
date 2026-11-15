using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Isv;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IIsvInfoRepository : IRepository<IsvInfo, string>
    {
        Task<bool> ExistsByIsvNoAsync(string isvNo, CancellationToken cancellationToken = default);
    }
}
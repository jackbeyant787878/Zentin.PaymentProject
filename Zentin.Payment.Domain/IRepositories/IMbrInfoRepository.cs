using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Member;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMbrInfoRepository : IRepository<MbrInfo, string>
    {
        Task<IReadOnlyList<MbrInfo>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<MbrInfo?> GetByTelAsync(string mchNo, string mbrTel, CancellationToken cancellationToken = default);

        Task<MbrInfo?> GetByWxOpenIdAsync(string mchNo, string wxMpOpenId, CancellationToken cancellationToken = default);

        Task<MbrInfo?> GetByAliUserIdAsync(string mchNo, string aliUserId, CancellationToken cancellationToken = default);
    }
}
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Member;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMbrRechargeRuleRepository : IRepository<MbrRechargeRule, long>
    {
        Task<IReadOnlyList<MbrRechargeRule>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MbrRechargeRule>> GetEnabledByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);
    }
}
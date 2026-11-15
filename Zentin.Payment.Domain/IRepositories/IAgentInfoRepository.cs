using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Agent;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IAgentInfoRepository : IRepository<AgentInfo, string>
    {
        Task<IReadOnlyList<AgentInfo>> GetByPidAsync(string pid, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<AgentInfo>> GetByIsvNoAsync(string isvNo, CancellationToken cancellationToken = default);

        Task<bool> ExistsByAgentNoAsync(string agentNo, CancellationToken cancellationToken = default);
    }
}
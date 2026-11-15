using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Agent;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class AgentInfoRepository : RepositoryBase<AgentInfo, string>, IAgentInfoRepository
    {
        public AgentInfoRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<AgentInfo>> GetByPidAsync(string pid, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(a => a.Pid == pid).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AgentInfo>> GetByIsvNoAsync(string isvNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(a => a.IsvNo == isvNo).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByAgentNoAsync(string agentNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(a => a.AgentNo == agentNo, cancellationToken);
        }
    }
}
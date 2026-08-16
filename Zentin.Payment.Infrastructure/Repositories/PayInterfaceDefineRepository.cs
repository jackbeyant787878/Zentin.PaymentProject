using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class PayInterfaceDefineRepository : RepositoryBase<PayInterfaceDefine, string>, IPayInterfaceDefineRepository
    {
        public PayInterfaceDefineRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<PayInterfaceDefine>> GetAvailableByWayCodeAsync(string wayCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(d => d.State == true && d.WayCodes.Contains(wayCode)).ToListAsync(cancellationToken);
        }
    }
}

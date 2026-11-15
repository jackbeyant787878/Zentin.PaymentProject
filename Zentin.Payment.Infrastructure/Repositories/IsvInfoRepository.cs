using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Isv;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class IsvInfoRepository : RepositoryBase<IsvInfo, string>, IIsvInfoRepository
    {
        public IsvInfoRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<bool> ExistsByIsvNoAsync(string isvNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(i => i.IsvNo == isvNo, cancellationToken);
        }
    }
}
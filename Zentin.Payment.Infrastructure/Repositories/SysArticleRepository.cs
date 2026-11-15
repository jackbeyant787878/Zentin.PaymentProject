using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Sys;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class SysArticleRepository : RepositoryBase<SysArticle, long>, ISysArticleRepository
    {
        public SysArticleRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<SysArticle>> GetPublishedAsync(int take, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking()
                .Where(a => a.PublishTime != null)
                .OrderByDescending(a => a.PublishTime)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
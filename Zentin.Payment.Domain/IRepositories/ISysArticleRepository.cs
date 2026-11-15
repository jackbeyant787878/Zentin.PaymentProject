using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Sys;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface ISysArticleRepository : IRepository<SysArticle, long>
    {
        Task<IReadOnlyList<SysArticle>> GetPublishedAsync(int take, CancellationToken cancellationToken = default);
    }
}
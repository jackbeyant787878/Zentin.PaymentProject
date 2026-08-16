using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayRateLevelConfigRepository : IRepository<PayRateLevelConfig, long>
    {
        Task<List<PayRateLevelConfig>> GetByRateConfigIdAsync(long rateConfigId, CancellationToken cancellationToken = default);
    }
}

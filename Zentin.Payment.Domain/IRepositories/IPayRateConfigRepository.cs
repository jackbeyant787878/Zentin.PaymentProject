using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayRateConfigRepository : IRepository<PayRateConfig, long>
    {
        Task<PayRateConfig?> GetByUniqueKeyAsync(string configType, string infoType, string infoId, string ifCode, string wayCode, CancellationToken cancellationToken = default);

        Task<List<PayRateConfig>> GetByInfoIdAsync(string configType, string infoType, string infoId, CancellationToken cancellationToken = default);

        Task<List<PayRateConfig>> GetByInfoIdAndIfCodeAsync(string configType, string infoType, string infoId, string ifCode, CancellationToken cancellationToken = default);
    }
}

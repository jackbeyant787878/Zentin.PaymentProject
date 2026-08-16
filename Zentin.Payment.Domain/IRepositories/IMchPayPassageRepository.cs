using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchPayPassageRepository : IRepository<MchPayPassage, long>
    {
        Task<List<MchPayPassage>> GetByMchAppAsync(string mchNo, string appId, CancellationToken cancellationToken = default);

        Task<bool> IsExistUseWayCodeAsync(string wayCode, CancellationToken cancellationToken = default);
    }
}

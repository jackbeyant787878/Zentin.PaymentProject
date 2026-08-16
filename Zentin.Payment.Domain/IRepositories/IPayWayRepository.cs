using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayWayRepository : IRepository<PayWay, string>
    {
        Task<bool> IsExistWayCodeAsync(string wayCode, CancellationToken cancellationToken = default);
    }
}

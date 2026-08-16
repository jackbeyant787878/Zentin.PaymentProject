using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayInterfaceDefineRepository : IRepository<PayInterfaceDefine, string>
    {
        Task<List<PayInterfaceDefine>> GetAvailableByWayCodeAsync(string wayCode, CancellationToken cancellationToken = default);
    }
}

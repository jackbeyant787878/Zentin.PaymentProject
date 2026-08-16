using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IPayInterfaceConfigRepository : IRepository<PayInterfaceConfig, long>
    {
        Task<PayInterfaceConfig?> GetByInfoAsync(string infoType, string infoId, string ifCode, CancellationToken cancellationToken = default);

        Task<bool> IsExistUseIfCodeAsync(string ifCode, CancellationToken cancellationToken = default);

        Task<bool> MchAppHasAvailableIfCodeAsync(string appId, string ifCode, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Device;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IDeviceProviderConfigRepository : IRepository<DeviceProviderConfig, long>
    {
        Task<DeviceProviderConfig?> GetByProviderAsync(int deviceType, string providerCode, CancellationToken cancellationToken = default);
    }
}

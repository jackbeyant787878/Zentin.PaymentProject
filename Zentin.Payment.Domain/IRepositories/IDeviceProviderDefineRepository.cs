using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Device;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IDeviceProviderDefineRepository : IRepository<DeviceProviderDefine, string>
    {
        Task<List<DeviceProviderDefine>> GetAvailableByDeviceTypeAsync(int deviceType, CancellationToken cancellationToken = default);
    }
}

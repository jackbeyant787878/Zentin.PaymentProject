using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Device;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IDeviceInfoRepository : IRepository<DeviceInfo, long>
    {
        Task<DeviceInfo?> GetByDeviceNoAsync(string providerCode, int deviceType, string deviceNo, CancellationToken cancellationToken = default);

        Task<List<DeviceInfo>> GetByStoreIdAsync(long storeId, CancellationToken cancellationToken = default);
    }
}

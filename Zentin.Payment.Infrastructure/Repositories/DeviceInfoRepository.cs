using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Device;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class DeviceInfoRepository :RepositoryBase<DeviceInfo, long>, IDeviceInfoRepository
    {
        public DeviceInfoRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<DeviceInfo?> GetByDeviceNoAsync(string providerCode, int deviceType, string deviceNo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(d =>d.ProviderCode == providerCode && (int)d.DeviceType == deviceType && d.DeviceNo == deviceNo, cancellationToken);
        }

        public async Task<List<DeviceInfo>> GetByStoreIdAsync(long storeId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(d => d.StoreId == storeId).ToListAsync(cancellationToken);
        }
    }
}

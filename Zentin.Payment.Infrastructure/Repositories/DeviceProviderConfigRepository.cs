using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Device;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class DeviceProviderConfigRepository : RepositoryBase<DeviceProviderConfig, long>, IDeviceProviderConfigRepository
    {
        public DeviceProviderConfigRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<DeviceProviderConfig?> GetByProviderAsync(int deviceType, string providerCode, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.DeviceType == (DeviceType)deviceType && c.ProviderCode == providerCode, cancellationToken);
        }
    }
}

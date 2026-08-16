using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Device;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class DeviceProviderDefineRepository : RepositoryBase<DeviceProviderDefine, string>, IDeviceProviderDefineRepository
    {
        public DeviceProviderDefineRepository(PaymentDbContext dbContext) : base(dbContext) { }

        public async Task<List<DeviceProviderDefine>> GetAvailableByDeviceTypeAsync(int deviceType, CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().Where(d => d.State == true && d.DeviceTypes.Contains(deviceType)).ToListAsync(cancellationToken);
        }
    }
}

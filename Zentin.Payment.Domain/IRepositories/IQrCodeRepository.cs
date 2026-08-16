using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.QrCode;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IQrCodeRepository : IRepository<QrCode, string>
    {
        Task<bool> IsExistBatchIdAsync(string batchId, CancellationToken cancellationToken = default);

        Task<List<QrCode>> GetByStoreIdAsync(long storeId, CancellationToken cancellationToken = default);
    }
}

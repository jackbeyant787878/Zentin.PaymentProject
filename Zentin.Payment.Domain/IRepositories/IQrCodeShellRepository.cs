using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.QrCode;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IQrCodeShellRepository : IRepository<QrCodeShell, int>
    {
        Task<List<QrCodeShell>> GetByBelongAsync(string sysType, string belongInfoId, CancellationToken cancellationToken = default);
    }
}

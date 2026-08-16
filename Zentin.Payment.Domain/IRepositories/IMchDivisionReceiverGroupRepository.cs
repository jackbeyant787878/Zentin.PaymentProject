using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Division;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchDivisionReceiverGroupRepository : IRepository<MchDivisionReceiverGroup, long>
    {
        Task<List<MchDivisionReceiverGroup>> GetByMchNoAsync(string mchNo, CancellationToken cancellationToken = default);
    }
}

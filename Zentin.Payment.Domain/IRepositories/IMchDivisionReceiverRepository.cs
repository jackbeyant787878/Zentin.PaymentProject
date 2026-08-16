using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Division;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IMchDivisionReceiverRepository : IRepository<MchDivisionReceiver, long>
    {
        Task<List<MchDivisionReceiver>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default);

        Task<bool> IsExistUseReceiverGroupAsync(long receiverGroupId, CancellationToken cancellationToken = default);
    }
}

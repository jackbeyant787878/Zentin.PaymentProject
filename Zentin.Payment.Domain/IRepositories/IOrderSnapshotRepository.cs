using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Entities.Order;

namespace Zentin.Payment.Domain.IRepositories
{
    public interface IOrderSnapshotRepository
    {
        Task<OrderSnapshot?> GetByIdAsync(string orderId, int orderType, CancellationToken cancellationToken = default);

        Task AddAsync(OrderSnapshot snapshot, CancellationToken cancellationToken = default);

        void Update(OrderSnapshot snapshot);
    }
}

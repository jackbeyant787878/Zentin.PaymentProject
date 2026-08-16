using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Domain.Entities.Order;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Repositories
{
    public sealed class OrderSnapshotRepository : IOrderSnapshotRepository
    {
        private readonly PaymentDbContext _dbContext;
        private readonly DbSet<OrderSnapshot> _dbSet;

        public OrderSnapshotRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<OrderSnapshot>();
        }

        public async Task<OrderSnapshot?> GetByIdAsync(string orderId, int orderType, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { orderId, orderType }, cancellationToken);
        }

        public async Task AddAsync(OrderSnapshot snapshot, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(snapshot, cancellationToken);
        }

        public void Update(OrderSnapshot snapshot)
        {
            _dbSet.Update(snapshot);
        }
    }
}

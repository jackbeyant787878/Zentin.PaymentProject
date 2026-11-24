using MediatR;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed class ClosePayOrderCommandHandler(
        IPayOrderRepository payOrderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ClosePayOrderCommand>
    {
        public async Task Handle(ClosePayOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await payOrderRepository.GetByIdAsync(request.PayOrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"支付订单不存在: {request.PayOrderId}");

            order.Close(request.Reason);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
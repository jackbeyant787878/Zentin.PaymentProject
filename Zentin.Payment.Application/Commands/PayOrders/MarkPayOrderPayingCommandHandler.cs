using MediatR;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed class MarkPayOrderPayingCommandHandler(
        IPayOrderRepository payOrderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<MarkPayOrderPayingCommand>
    {
        public async Task Handle(MarkPayOrderPayingCommand request, CancellationToken cancellationToken)
        {
            var order = await payOrderRepository.GetByIdAsync(request.PayOrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"支付订单不存在: {request.PayOrderId}");

            order.MarkPaying(request.IfCode);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
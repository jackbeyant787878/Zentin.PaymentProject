using MediatR;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed class ConfirmPayOrderSuccessCommandHandler(
        IPayOrderRepository payOrderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ConfirmPayOrderSuccessCommand>
    {
        public async Task Handle(ConfirmPayOrderSuccessCommand request, CancellationToken cancellationToken)
        {
            var order = await payOrderRepository.GetByIdAsync(request.PayOrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"支付订单不存在: {request.PayOrderId}");

            order.MarkSuccess(request.ChannelOrderNo, request.ChannelUser, request.PlatformOrderNo);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
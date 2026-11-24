using MediatR;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed class ConfirmPayOrderFailCommandHandler(
        IPayOrderRepository payOrderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ConfirmPayOrderFailCommand>
    {
        public async Task Handle(ConfirmPayOrderFailCommand request, CancellationToken cancellationToken)
        {
            var order = await payOrderRepository.GetByIdAsync(request.PayOrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"支付订单不存在: {request.PayOrderId}");

            order.MarkFail(request.ErrCode, request.ErrMsg);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
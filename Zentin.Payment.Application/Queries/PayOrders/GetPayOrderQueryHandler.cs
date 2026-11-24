using MediatR;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Application.Mapping;
using Zentin.Payment.Domain.IRepositories;

namespace Zentin.Payment.Application.Queries.PayOrders
{
    public sealed class GetPayOrderQueryHandler(
        IPayOrderRepository payOrderRepository) : IRequestHandler<GetPayOrderQuery, PayOrderDto>
    {
        public async Task<PayOrderDto> Handle(GetPayOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await payOrderRepository.GetByIdAsync(request.PayOrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"支付订单不存在: {request.PayOrderId}");

            return order.ToDto();
        }
    }
}
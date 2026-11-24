using MediatR;
using Zentin.Payment.Application.Dto;

namespace Zentin.Payment.Application.Queries.PayOrders
{
    public sealed record GetPayOrderQuery(string PayOrderId) : IRequest<PayOrderDto>;
}
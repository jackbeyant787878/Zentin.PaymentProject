using MediatR;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed record CreatePayOrderCommand : IRequest<CreatePayOrderResult>
    {
        public required string MchNo { get; init; }
        public required string MchName { get; init; }
        public required string AppId { get; init; }
        public required string MchOrderNo { get; init; }
        public required string WayCode { get; init; }
        public required string WayType { get; init; }
        public required long Amount { get; init; }
        public required decimal MchFeeRate { get; init; }
        public required string Subject { get; init; }
        public required string Body { get; init; }
        public required string NotifyUrl { get; init; }
        public required DateTime ExpiredTime { get; init; }
        public long? StoreId { get; init; }
        public string? ClientIp { get; init; }
        public string? ExtParam { get; init; }
        public string? ReturnUrl { get; init; }
    }

    public sealed record CreatePayOrderResult(string PayOrderId, PayOrderStatus State);
}
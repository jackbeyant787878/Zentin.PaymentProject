using FluentValidation;
using MediatR;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed record ConfirmPayOrderSuccessCommand(
        string PayOrderId,
        string ChannelOrderNo,
        string? ChannelUser = null,
        string? PlatformOrderNo = null) : IRequest;

    public sealed class ConfirmPayOrderSuccessCommandValidator : AbstractValidator<ConfirmPayOrderSuccessCommand>
    {
        public ConfirmPayOrderSuccessCommandValidator()
        {
            RuleFor(x => x.PayOrderId).NotEmpty().MaximumLength(30);
            RuleFor(x => x.ChannelOrderNo).NotEmpty().MaximumLength(64);
            RuleFor(x => x.ChannelUser).MaximumLength(64);
            RuleFor(x => x.PlatformOrderNo).MaximumLength(64);
        }
    }
}
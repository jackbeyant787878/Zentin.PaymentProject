using FluentValidation;
using MediatR;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed record ClosePayOrderCommand(string PayOrderId, string? Reason = null) : IRequest;

    public sealed class ClosePayOrderCommandValidator : AbstractValidator<ClosePayOrderCommand>
    {
        public ClosePayOrderCommandValidator()
        {
            RuleFor(x => x.PayOrderId).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Reason).MaximumLength(256);
        }
    }
}
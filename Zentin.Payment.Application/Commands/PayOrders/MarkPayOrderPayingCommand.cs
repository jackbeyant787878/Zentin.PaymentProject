using FluentValidation;
using MediatR;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed record MarkPayOrderPayingCommand(string PayOrderId, string IfCode) : IRequest;

    public sealed class MarkPayOrderPayingCommandValidator : AbstractValidator<MarkPayOrderPayingCommand>
    {
        public MarkPayOrderPayingCommandValidator()
        {
            RuleFor(x => x.PayOrderId).NotEmpty().MaximumLength(30);
            RuleFor(x => x.IfCode).NotEmpty().MaximumLength(20);
        }
    }
}
using FluentValidation;
using MediatR;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed record ConfirmPayOrderFailCommand(string PayOrderId, string ErrCode, string ErrMsg) : IRequest;

    public sealed class ConfirmPayOrderFailCommandValidator : AbstractValidator<ConfirmPayOrderFailCommand>
    {
        public ConfirmPayOrderFailCommandValidator()
        {
            RuleFor(x => x.PayOrderId).NotEmpty().MaximumLength(30);
            RuleFor(x => x.ErrCode).NotEmpty().MaximumLength(128);
            RuleFor(x => x.ErrMsg).NotEmpty().MaximumLength(256);
        }
    }
}
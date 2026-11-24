using FluentValidation;

namespace Zentin.Payment.Application.Commands.PayOrders
{
    public sealed class CreatePayOrderCommandValidator : AbstractValidator<CreatePayOrderCommand>
    {
        public CreatePayOrderCommandValidator()
        {
            RuleFor(x => x.MchNo).NotEmpty().MaximumLength(64);
            RuleFor(x => x.MchName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.AppId).NotEmpty().MaximumLength(64);
            RuleFor(x => x.MchOrderNo).NotEmpty().MaximumLength(64);
            RuleFor(x => x.WayCode).NotEmpty().MaximumLength(20);
            RuleFor(x => x.WayType).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.MchFeeRate).InclusiveBetween(0m, 1m);
            RuleFor(x => x.Subject).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(256);
            // 供体 AbstractPayOrderController.cs:138-145 对通知/返回地址做 URL 合法性校验
            RuleFor(x => x.NotifyUrl).NotEmpty().MaximumLength(128)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                .WithMessage("NotifyUrl 必须是 http/https 绝对地址");
            RuleFor(x => x.ExpiredTime).GreaterThan(DateTime.UtcNow);
        }
    }
}
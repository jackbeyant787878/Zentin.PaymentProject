using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Refund;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Enumerator;
using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.Channel
{
    public abstract class AbstractRefundService : IRefundService
    {
        protected readonly ILogger<AbstractRefundService> _logger;
        protected readonly ISysConfigService _sysConfigService;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ConfigContextQueryService _configContextQueryService;
        protected AbstractRefundService(ILogger<AbstractRefundService> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configContextQueryService = configContextQueryService;
            _sysConfigService = sysConfigService;
        }

        protected AbstractRefundService()
        {
        }

        public abstract string GetIfCode();
        public abstract string PreCheck(RefundOrderRQ bizRQ, RefundOrderDto refundOrder, PayOrderDto payOrder);

        public virtual long CalculateFeeAmount(long amount, PayOrderDto payOrder)
        {
            var refundState = payOrder.RefundAmount + amount >= payOrder.Amount ? PayOrderRefund.REFUND_STATE_ALL : PayOrderRefund.REFUND_STATE_SUB;
            if (refundState.Equals(PayOrderRefund.REFUND_STATE_ALL))
            {
                return payOrder.MchFeeAmount;
            }
            return AmountUtil.CalPercentageFee(amount, payOrder.MchFeeRate);
        }

        public abstract Task<ChannelRetMsg> QueryAsync(RefundOrderDto refundOrder, MchAppConfigContext mchAppConfigContext);
        public abstract Task<ChannelRetMsg> RefundAsync(RefundOrderRQ bizRQ, RefundOrderDto refundOrder, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext);

        protected string GetNotifyUrl()
        {
            return $"{_sysConfigService.GetDBApplicationConfig().PaySiteUrl}/api/refund/notify/{GetIfCode()}";
        }

        protected string GetNotifyUrl(string refundOrderId)
        {
            return $"{_sysConfigService.GetDBApplicationConfig().PaySiteUrl}/api/refund/notify/{GetIfCode()}/{refundOrderId}";
        }
    }
}

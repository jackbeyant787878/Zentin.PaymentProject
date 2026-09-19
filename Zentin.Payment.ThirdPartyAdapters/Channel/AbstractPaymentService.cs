using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Enumerator;
using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.Channel
{
    /// <summary>
    /// 支付接口抽象类
    /// </summary>
    public abstract class AbstractPaymentService : IPaymentService
    {
        protected readonly ILogger<AbstractPaymentService> _logger;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ISysConfigService _sysConfigService;
        protected readonly ConfigContextQueryService _configContextQueryService;
        protected AbstractPaymentService(ILogger<AbstractPaymentService> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sysConfigService = sysConfigService;
            _configContextQueryService = configContextQueryService;
        }

        protected AbstractPaymentService()
        {
        }

        public abstract string GetIfCode();
        public abstract bool IsSupport(string wayCode);
        public abstract Task<AbstractRS> PayAsync(UnifiedOrderRQ bizRQ, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext);
        public abstract Task<string> PreCheckAsync(UnifiedOrderRQ bizRQ, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext);

        public virtual long CalculateFeeAmount(long amount, decimal rate)
        {
            return AmountUtil.CalPercentageFee(amount, rate);
        }

        public virtual long CalculateProfitAmount(long amount, decimal rate)
        {
            return AmountUtil.CalPercentageFee(amount, rate);
        }

        /// <summary>
        /// 订单分账（一般用作 如微信订单将在下单处做标记）
        /// </summary>
        /// <param name="payOrder"></param>
        /// <returns></returns>
        protected bool IsDivisionOrder(PayOrderDto payOrder)
        {
            //订单分账， 将冻结商户资金。
            if (payOrder.DivisionMode != null && ((byte)PayOrderDivisionMode.DIVISION_MODE_AUTO == payOrder.DivisionMode || (byte)PayOrderDivisionMode.DIVISION_MODE_MANUAL == payOrder.DivisionMode))
            {
                return true;
            }
            return false;
        }

        protected string GetNotifyUrl()
        {
            return $"{_sysConfigService.GetDBApplicationConfig().PaySiteUrl}/api/pay/notify/{GetIfCode()}";
        }

        protected string GetNotifyUrl(string payOrderId)
        {
            return $"{_sysConfigService.GetDBApplicationConfig().PaySiteUrl}/api/pay/notify/{GetIfCode()}/{payOrderId}";
        }

        protected string GetReturnUrl()
        {
            return $"{_sysConfigService.GetDBApplicationConfig().PaySiteUrl}/api/pay/return/{GetIfCode()}";
        }

        protected string GetReturnUrl(string payOrderId)
        {
            return $"{_sysConfigService.GetDBApplicationConfig().PaySiteUrl}/api/pay/return/{GetIfCode()}/{payOrderId}";
        }
    }
}

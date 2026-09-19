using  Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder;
using Zentin.Payment.ThirdPartyAdapters.Services;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.WxPay.PayWayV3
{
    /// <summary>
    /// 微信 条码支付
    /// </summary>
    public class WxBar : WxPayPaymentService
    {
        private readonly IPaymentService _paymentService;

        public WxBar(ILogger<WxBar> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
            //_paymentService = serviceProvider.GetServices<IPaymentService>().FirstOrDefault(f => f.GetType().Equals(typeof(PayWay.WxBar)));
            //_paymentService = serviceProvider.GetRequiredService<PayWay.WxBar>();
            //_paymentService = ActivatorUtilities.CreateInstance<PayWay.WxBar>(serviceProvider);

            _paymentService = serviceProvider.GetRequiredKeyedService<IPaymentService>(typeof(PayWay.WxBar).FullName.ToLower());
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return _paymentService.PreCheckAsync(rq, payOrder, mchAppConfigContext);
        }

        public override Task<AbstractRS> PayAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return _paymentService.PayAsync(rq, payOrder, mchAppConfigContext);
        }
    }
}

using Zentin.Payment.ThirdPartyAdapters.Channel.AliPay.Kits;
using Zentin.Payment.ThirdPartyAdapters.Exceptions;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.AliPay.PayWay
{
    /// <summary>
    /// 支付宝 APP支付
    /// </summary>
    public class AliApp : AliPayPaymentService
    {
        public AliApp(ILogger<AliApp> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
        }

        public override async Task<AbstractRS> PayAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            AlipayTradeAppPayRequest req = new AlipayTradeAppPayRequest();
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.OutTradeNo = payOrder.PayOrderId;
            model.Subject = payOrder.Subject; //订单标题
            model.Body = payOrder.Body; //订单描述信息
            model.TotalAmount = AmountUtil.ConvertCent2Dollar(payOrder.Amount);  //支付金额
            req.SetNotifyUrl(GetNotifyUrl()); // 设置异步通知地址
            req.SetBizModel(model);

            //统一放置 isv接口必传信息
            await AliPayKit.PutApiIsvInfoAsync(mchAppConfigContext, req, model);


            string payData;

            // sdk方式需自行拦截接口异常信息
            try
            {
                payData = (await _configContextQueryService.GetAlipayClientWrapperAsync(mchAppConfigContext)).AlipayClient.SdkExecute(req).Body;
            }
            catch (AopException e)
            {
                throw ChannelException.SysError(e.Message);
            }

            // 构造函数响应数据
            AliAppOrderRS res = ApiResBuilder.BuildSuccess<AliAppOrderRS>();
            ChannelRetMsg channelRetMsg = new ChannelRetMsg();
            res.ChannelRetMsg = channelRetMsg;

            //放置 响应数据
            channelRetMsg.ChannelAttach = payData;
            channelRetMsg.ChannelState = ChannelState.WAITING;
            res.PayData = payData;
            return res;
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return Task.FromResult<string>(null);
        }
    }
}

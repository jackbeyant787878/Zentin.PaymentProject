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
using Zentin.Payment.Common.Constant;
using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.AliPay.PayWay
{
    /// <summary>
    /// 支付宝 Pc支付
    /// </summary>
    public class AliPc : AliPayPaymentService
    {
        public AliPc(ILogger<AliPc> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
        }

        public override async Task<AbstractRS> PayAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            AliPcOrderRQ bizRQ = (AliPcOrderRQ)rq;

            AlipayTradePagePayRequest req = new AlipayTradePagePayRequest();
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.OutTradeNo = payOrder.PayOrderId;
            model.Subject = payOrder.Subject; //订单标题
            model.Body = payOrder.Body; //订单描述信息
            model.TotalAmount = AmountUtil.ConvertCent2Dollar(payOrder.Amount);  //支付金额
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";
            model.QrPayMode = "2"; //订单码-跳转模式
            req.SetNotifyUrl(GetNotifyUrl()); // 设置异步通知地址
            req.SetReturnUrl(GetReturnUrl()); // 同步跳转地址
            req.SetBizModel(model);

            //统一放置 isv接口必传信息
            await AliPayKit.PutApiIsvInfoAsync(mchAppConfigContext, req, model);

            // 构造函数响应数据
            AliPcOrderRS res = ApiResBuilder.BuildSuccess<AliPcOrderRS>();

            // sdk方式需自行拦截接口异常信息
            try
            {
                var alipayClientWrapper = await _configContextQueryService.GetAlipayClientWrapperAsync(mchAppConfigContext);
                if (Constants.PAY_DATA_TYPE.FORM.Equals(bizRQ.PayDataType))
                {
                    res.FormContent = alipayClientWrapper.AlipayClient.pageExecute(req).Body;
                }
                else
                {
                    res.PayUrl = alipayClientWrapper.AlipayClient.pageExecute(req, null, "GET").Body;
                }
            }
            catch (AopException e)
            {
                throw ChannelException.SysError(e.Message);
            }

            ChannelRetMsg channelRetMsg = new ChannelRetMsg();
            res.ChannelRetMsg = channelRetMsg;

            //放置 响应数据
            channelRetMsg.ChannelState = ChannelState.WAITING;
            return res;
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return Task.FromResult<string>(null);
        }
    }
}

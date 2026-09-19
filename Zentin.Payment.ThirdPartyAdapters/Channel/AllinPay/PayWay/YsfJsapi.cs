using  Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Utils;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;
using Newtonsoft.Json.Linq;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.AllinPay.PayWay
{
    /// <summary>
    /// 通联 云闪付 jsapi
    /// </summary>
    public class YsfJsapi : AllinPayPaymentService
    {
        public YsfJsapi(ILogger<YsfJsapi> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
        }

        public override async Task<AbstractRS> PayAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            string logPrefix = "【通联(unionpay)jsapi支付】";
            YsfJsapiOrderRQ bizRQ = (YsfJsapiOrderRQ)rq;
            JObject reqParams = new JObject();
            YsfJsapiOrderRS res = ApiResBuilder.BuildSuccess<YsfJsapiOrderRS>();
            ChannelRetMsg channelRetMsg = new ChannelRetMsg();
            res.ChannelRetMsg = channelRetMsg;

            // 请求参数赋值
            UnifiedParamsSet(reqParams, payOrder, GetNotifyUrl(), GetReturnUrl());
            //通联扫一扫支付， 需要传入buyerUserId参数
            /*用户号（微信openid / 支付宝userid / 银联userid）
            payType == "WECHAT"或"ALIPAY"时必传*/
            reqParams.Add("acct", bizRQ.GetChannelUserId());

            // 获取微信官方配置的 appId
            //AllinPayIsvSubMchParams allinpayIsvParams = (AllinPayIsvSubMchParams)_configContextQueryService.QueryIsvSubMchParams(mchAppConfigContext.MchNo, mchAppConfigContext.AppId, GetIfCode());
            reqParams.Add("sub_appid", "");

            // 发送请求
            JObject resJSON = await PackageParamAndReqAsync("/apiweb/unitorder/pay", reqParams, logPrefix, mchAppConfigContext);
            //请求 & 响应成功， 判断业务逻辑
            string code = resJSON.GetValue("retcode").ToString(); //请求响应码
            string msg = resJSON.GetValue("retmsg").ToString(); //响应信息
            reqParams.TryGetString("cusid", out string cusid); // 商户号
            channelRetMsg.ChannelMchNo = cusid;
            //channelRetMsg.ChannelIsvNo = orgid;
            try
            {
                if ("SUCCESS".Equals(code))
                {
                    string trxid = resJSON.GetValue("trxid").ToString();
                    string payinfo = resJSON.GetValue("payinfo").ToString();
                    res.RedirectUrl = payinfo;
                    channelRetMsg.ChannelOrderId = trxid;
                    channelRetMsg.ChannelState = ChannelState.WAITING;
                }
                else
                {
                    channelRetMsg.ChannelState = ChannelState.WAITING;
                    channelRetMsg.IsNeedQuery = true; // 开启轮询查单
                }
            }
            catch (Exception)
            {
                channelRetMsg.ChannelErrCode = code;
                channelRetMsg.ChannelErrMsg = msg;
            }
            return res;
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return Task.FromResult<string>(null);
        }
    }
}

using  Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.Common.Utils;
using Zentin.Payment.ThirdPartyAdapters.Channel.LConstantswPay.Enumerator;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;
using Newtonsoft.Json.Linq;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.LConstantswPay.PayWay
{
    /// <summary>
    /// 利楚扫呗 支付宝 二维码支付
    /// </summary>
    public class AliQr : LConstantswPayPaymentService
    {
        public AliQr(ILogger<AliQr> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
        }

        public override async Task<AbstractRS> PayAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            string logPrefix = "【利楚扫呗(alipay)二维码支付】";
            AliQrOrderRQ bizRQ = (AliQrOrderRQ)rq;
            JObject reqParams = new JObject();
            AliQrOrderRS res = ApiResBuilder.BuildSuccess<AliQrOrderRS>();
            ChannelRetMsg channelRetMsg = new ChannelRetMsg();
            res.ChannelRetMsg = channelRetMsg;

            // 请求参数赋值
            string payType = LConstantswPayEnum.GetPayType(payOrder.WayCode);
            reqParams.Add("pay_ver", "201");
            reqParams.Add("pay_type", payType);
            reqParams.Add("service_id", "011");
            reqParams.Add("notify_url", GetNotifyUrl()); //支付结果通知地址不上送则交易成功后，无异步交易结果通知
            PublicParams(reqParams, payOrder);

            // 发送请求
            JObject resJSON = await PackageParamAndReqAsync("/pay/open/prepay", reqParams, logPrefix, mchAppConfigContext);
            //请求 & 响应成功， 判断业务逻辑
            string returnCode = resJSON.GetValue("return_code").ToString(); //请求响应码
            string returnMsg = resJSON.GetValue("return_msg").ToString(); //响应信息
            resJSON.TryGetString("merchant_no", out string merchantNo); // 商户号
            channelRetMsg.ChannelMchNo = merchantNo;
            try
            {
                if ("01".Equals(returnCode))
                {
                    resJSON.TryGetString("result_code", out string resultCode); // 业务结果
                    if ("01".Equals(resultCode))
                    {
                        string outTradeNo = resJSON.GetValue("out_trade_no").ToString();//平台唯一订单号
                        string qrCode = resJSON.GetValue("qr_code").ToString();
                        //二维码地址
                        if (Constants.PAY_DATA_TYPE.CODE_IMG_URL.Equals(bizRQ.PayDataType))
                        {
                            res.CodeImgUrl = _sysConfigService.GetDBApplicationConfig().GenScanImgUrl(qrCode);
                        }
                        else
                        {
                            //默认都为跳转地址方式
                            res.CodeUrl = qrCode;
                        }
                        channelRetMsg.ChannelOrderId = outTradeNo;
                        channelRetMsg.ChannelState = ChannelState.WAITING;
                    }
                    else
                    {
                        channelRetMsg.ChannelState = ChannelState.CONFIRM_FAIL;
                        channelRetMsg.ChannelErrCode = resultCode;
                        channelRetMsg.ChannelErrMsg = returnMsg;
                    }
                }
                else
                {
                    channelRetMsg.ChannelState = ChannelState.CONFIRM_FAIL;
                    channelRetMsg.ChannelErrCode = returnCode;
                    channelRetMsg.ChannelErrMsg = returnMsg;
                }
            }
            catch (Exception)
            {
                channelRetMsg.ChannelErrCode = returnCode;
                channelRetMsg.ChannelErrMsg = returnMsg;
            }
            return res;
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return Task.FromResult<string>(null);
        }
    }
}

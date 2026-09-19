using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.AllinPay.Enumerator
{
    public class AllinPayEnum
    {
        public enum PayType
        {
            /// <summary>
            /// 微信扫码支付
            /// </summary>
            W01,
            /// <summary>
            /// 微信JS支付
            /// </summary>
            W02,
            /// <summary>
            /// 微信APP支付
            /// </summary>
            W03,
            /// <summary>
            /// 微信小程序支付
            /// </summary>
            W06,
            /// <summary>
            /// 支付宝扫码支付
            /// </summary>
            A01,
            /// <summary>
            /// 支付宝JS支付
            /// </summary>
            A02,
            /// <summary>
            /// 支付宝APP支付
            /// </summary>
            A03,
            /// <summary>
            /// 银联扫码支付(ConstantsB)
            /// </summary>
            U01,
            /// <summary>
            /// 银联JS支付
            /// </summary>
            U02,
            /// <summary>
            /// 数币扫码支付
            /// </summary>
            S01,
            /// <summary>
            /// 数字货币H5
            /// </summary>
            S03,
            /// <summary>
            /// 网联支付
            /// </summary>
            N03
        }

        /// <summary>
        /// 支付渠道，枚举值
        /// 取值范围：
        /// WECHAT 微信
        /// ALIPAY 支付宝
        /// UNIONPAY 银联
        /// </summary>
        /// <param name="wayCode"></param>
        /// <returns></returns>
        public static string GetPayType(string wayCode)
        {
            string payType = null;
            switch (wayCode)
            {
                case Constants.PAY_WAY_CODE.ALI_LITE:
                case Constants.PAY_WAY_CODE.ALI_JSAPI:
                    payType = PayType.A02.ToString(); break;
                case Constants.PAY_WAY_CODE.ALI_APP:
                    payType = PayType.A03.ToString(); break;
                case Constants.PAY_WAY_CODE.ALI_PC:
                case Constants.PAY_WAY_CODE.ALI_WAP:
                case Constants.PAY_WAY_CODE.ALI_QR:
                    payType = PayType.A01.ToString(); break;
                case Constants.PAY_WAY_CODE.WX_NATIVE:
                    payType = PayType.W01.ToString(); break;
                case Constants.PAY_WAY_CODE.WX_H5:
                case Constants.PAY_WAY_CODE.WX_JSAPI:
                    payType = PayType.W02.ToString(); break;
                case Constants.PAY_WAY_CODE.WX_APP:
                    payType = PayType.W03.ToString(); break;
                case Constants.PAY_WAY_CODE.WX_LITE:
                    payType = PayType.W06.ToString(); break;
                case Constants.PAY_WAY_CODE.UP_QR:
                    payType = PayType.U01.ToString(); break;
                case Constants.PAY_WAY_CODE.UP_JSAPI:
                case Constants.PAY_WAY_CODE.YSF_JSAPI:
                    payType = PayType.U02.ToString(); break;
                case Constants.PAY_WAY_CODE.DCEP_QR:
                    payType = PayType.S01.ToString(); break;
                default:
                    break;
            }
            return payType;
        }
    }
}

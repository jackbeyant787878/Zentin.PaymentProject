using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.YsfPay.Enumerator
{
    public class YsfPayEnum
    {
        public interface OrderType
        {
            public const string ALIPAY = "alipay";

            public const string WECHAT = "wechat";

            public const string UNIONPAY = "unionpay";

            public const string ALIPAY_JS = "alipayJs";

            public const string WECHAT_JS = "wechatJs";

            public const string UP_JS = "upJs";
        }

        /// <summary>
        /// 云闪付条码付 封装参数，orderType
        /// </summary>
        /// <param name="wayCode"></param>
        /// <returns></returns>
        public static string GetOrderTypeByBar(string wayCode)
        {
            if (Constants.PAY_WAY_CODE.ALI_BAR.Equals(wayCode))
            {
                return OrderType.ALIPAY;
            }
            else if (Constants.PAY_WAY_CODE.WX_BAR.Equals(wayCode))
            {
                return OrderType.WECHAT;
            }
            else if (Constants.PAY_WAY_CODE.YSF_BAR.Equals(wayCode))
            {
                return OrderType.UNIONPAY;
            }

            return null;
        }

        /// <summary>
        /// 云闪付jsapi对应的订单类型
        /// </summary>
        /// <param name="wayCode"></param>
        /// <returns></returns>
        public static string GetOrderTypeByJSapi(string wayCode)
        {
            if (Constants.PAY_WAY_CODE.ALI_JSAPI.Equals(wayCode))
            {
                return OrderType.ALIPAY_JS;
            }
            else if (Constants.PAY_WAY_CODE.WX_JSAPI.Equals(wayCode))
            {
                return OrderType.WECHAT_JS;
            }
            else if (Constants.PAY_WAY_CODE.YSF_JSAPI.Equals(wayCode))
            {
                return OrderType.UP_JS;
            }

            return null;
        }

        /// <summary>
        /// 云闪付通用订单类型， 如查单
        /// </summary>
        /// <param name="wayCode"></param>
        /// <returns></returns>
        public static string GetOrderTypeByCommon(string wayCode)
        {
            if (Constants.PAY_WAY_CODE.ALI_JSAPI.Equals(wayCode) || Constants.PAY_WAY_CODE.ALI_BAR.Equals(wayCode))
            {
                return OrderType.ALIPAY;
            }
            else if (Constants.PAY_WAY_CODE.WX_JSAPI.Equals(wayCode) || Constants.PAY_WAY_CODE.WX_BAR.Equals(wayCode))
            {
                return OrderType.WECHAT;
            }
            else if (Constants.PAY_WAY_CODE.YSF_JSAPI.Equals(wayCode) || Constants.PAY_WAY_CODE.YSF_BAR.Equals(wayCode))
            {
                return OrderType.UNIONPAY;
            }
            return null;
        }
    }
}

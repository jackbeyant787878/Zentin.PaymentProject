using Newtonsoft.Json;
using Zentin.Payment.Common.Extensions;

namespace Zentin.Payment.Application.XxPay
{
    /// <summary>
    /// 小新支付 普通商户参数定义
    /// </summary>
    public class XxPayNormalMchParams : NormalMchParams
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 支付网关地址
        /// </summary>
        public string PayUrl { get; set; }

        public override string DeSenData()
        {
            if (!string.IsNullOrWhiteSpace(Key))
            {
                Key = Key.Mask();
            }
            return JsonConvert.SerializeObject(this);
        }
    }
}

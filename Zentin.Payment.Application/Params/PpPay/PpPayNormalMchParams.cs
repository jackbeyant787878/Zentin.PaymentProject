using Newtonsoft.Json;
using Zentin.Payment.Common.Extensions;

namespace Zentin.Payment.Application.PpPay
{
    public class PpPayNormalMchParams : NormalMchParams
    {
        /**
         * 是否沙箱环境
         */
        public byte? Sandbox { get; set; }

        /**
         * clientId
         * 客户端 ID
         */
        public string ClientId { get; set; }

        /**
         * secret
         * 密钥
         */
        public string Secret { get; set; }

        /**
         * 支付 Webhook 通知 ID
         */
        public string NotifyWebhook { get; set; }

        /**
         * 退款 Webhook 通知 ID
         */
        public string RefundWebhook { get; set; }

        public override string DeSenData()
        {
            if (!string.IsNullOrWhiteSpace(Secret))
            {
                Secret = Secret.Mask();
            }
            return JsonConvert.SerializeObject(this);
        }
    }
}

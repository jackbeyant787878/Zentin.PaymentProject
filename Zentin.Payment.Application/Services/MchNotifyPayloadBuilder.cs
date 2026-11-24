using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Zentin.Payment.Domain.Entities.Order;

namespace Zentin.Payment.Application.Services
{
    /// <summary>
    /// Builds the signed merchant notify body (POST_JSON default).
    /// Ported from donor QueryPayOrderRS.BuildByPayOrder + PayMchNotifyService.GenNotifyUrlAndBody + AgPayUtil.GetSign.
    /// </summary>
    public static class MchNotifyPayloadBuilder
    {
        public static string BuildSignedJsonBody(PayOrder order, string appSecret)
        {
            var json = new JObject
            {
                ["PayOrderId"] = order.PayOrderId,
                ["MchNo"] = order.MchNo,
                ["AppId"] = order.AppId,
                ["MchOrderNo"] = order.MchOrderNo,
                ["IfCode"] = order.IfCode,
                ["WayCode"] = order.WayCode,
                ["Amount"] = order.Amount,
                ["Currency"] = order.Currency,
                ["State"] = (byte)order.State,
                ["ClientIp"] = order.ClientIp,
                ["Subject"] = order.Subject,
                ["Body"] = order.Body,
                ["ChannelOrderNo"] = order.ChannelOrderNo,
                ["ErrCode"] = order.ErrCode,
                ["ErrMsg"] = order.ErrMsg,
                ["ExtParam"] = order.ExtParam,
                ["SuccessTime"] = ToUnixSeconds(order.SuccessTime),
                ["CreatedAt"] = ToUnixSeconds(order.CreatedOnUtc),
                ["reqTime"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            json["sign"] = GetSign(json, appSecret);
            return json.ToString(Newtonsoft.Json.Formatting.None);
        }

        private static long? ToUnixSeconds(DateTime? value)
        {
            if (value == null) return null;
            // EF 取回的 DateTime Kind 为 Unspecified，须显式标记 UTC 再转 epoch
            return new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)).ToUnixTimeSeconds();
        }

        // 供体 AgPayUtil.GetStrSort/GetSign 原样移植：
        // 跳过 null/空串/sign，默认字符串比较器排序，"&" 连接后直接追加 "key={secret}"（无 "&" 分隔），MD5 大写
        private static string GetSign(JObject json, string appSecret)
        {
            var sorted = json.Properties()
                .Where(p => !string.IsNullOrEmpty(p.Value.ToString()) && p.Name != "sign")
                .OrderBy(p => p.Name)
                .Select(p => $"{p.Name}={p.Value}");
            var source = string.Join("&", sorted) + $"key={appSecret}";
            return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(source)));
        }
    }
}
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Utilities;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.WxPay.Utils
{
    public class WxPayV3Util
    {
        public static string RSASign(string plainText, string privateKey)
        {
            return RSAUtility.SignWithSHA256(privateKey, plainText);
        }
    }
}

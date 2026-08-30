using Newtonsoft.Json.Linq;

namespace Zentin.Payment.Common.Utils
{
    public static class JsonUtil
    {
        public static JObject NewJson(string key, object val)
        {
            JObject result = new JObject();
            result.Add(key, JToken.FromObject(val));
            return result;
        }
    }
}

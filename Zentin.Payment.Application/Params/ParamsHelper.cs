using Newtonsoft.Json;

namespace Zentin.Payment.Application
{
    public static class ParamsHelper
    {
        public static T GetParams<T>(string ifCode, string paramsStr) where T : class
        {
            try
            {
                Type type = typeof(T);
                string typeName = $"{type.Namespace}.{ifCode}.{ifCode}{type.Name}";
                Type targetType = Type.GetType(typeName, false, true);
                return JsonConvert.DeserializeObject(paramsStr, targetType) as T;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

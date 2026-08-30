using Newtonsoft.Json;

namespace Zentin.Payment.Application
{
    public static class ParamsExtension
    {
        public static T GetParams<T>(this string paramsStr, string ifCode) where T : class
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

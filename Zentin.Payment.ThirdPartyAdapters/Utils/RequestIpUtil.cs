using Microsoft.Extensions.Primitives;

namespace Zentin.Payment.ThirdPartyAdapters.Utils
{
    public class RequestIpUtil
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestIpUtil(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRequestIP()
        {
            string ip = SplitConstantsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip))
                ip = SplitConstantsv(GetHeaderValueAs<string>("X-Real-IP")).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip) && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }

            return default;
        }

        public static List<string> SplitConstantsv(string ConstantsvList)
        {
            if (string.IsNullOrWhiteSpace(ConstantsvList))
                return new List<string>();

            return ConstantsvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}

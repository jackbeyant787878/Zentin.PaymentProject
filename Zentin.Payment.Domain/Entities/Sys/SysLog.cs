using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysLog : BaseEntity
    {
        public long SysLogId { get; private set; }
        public long? UserId { get; private set; }
        public string? UserName { get; private set; }
        public string Browser { get; private set; } = string.Empty;
        public string Os { get; private set; } = string.Empty;
        public string Device { get; private set; } = string.Empty;
        public string BrowserInfo { get; private set; } = string.Empty;
        public string UserIp { get; private set; } = string.Empty;
        public string SysType { get; private set; } = string.Empty;
        public string MethodName { get; private set; } = string.Empty;
        public byte LogType { get; private set; }
        public string MethodRemark { get; private set; } = string.Empty;
        public string ReqUrl { get; private set; } = string.Empty;
        public string ReqMethod { get; private set; } = string.Empty;
        public string OptReqParam { get; private set; } = string.Empty;
        public string OptResInfo { get; private set; } = string.Empty;
        public long? ElapsedMs { get; private set; }

        private SysLog()
        {
            SysLogId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static SysLog Record(
            string sysType,
            string methodName,
            byte logType,
            string reqUrl,
            string reqMethod,
            string userIp,
            string methodRemark = "",
            string optReqParam = "",
            string optResInfo = "",
            long? userId = null,
            string? userName = null,
            string browser = "",
            string os = "",
            string device = "",
            string browserInfo = "",
            long? elapsedMs = null)
        {
            return new SysLog
            {
                SysType = sysType,
                MethodName = methodName,
                LogType = logType,
                ReqUrl = reqUrl,
                ReqMethod = reqMethod,
                UserIp = userIp,
                MethodRemark = methodRemark,
                OptReqParam = optReqParam,
                OptResInfo = optResInfo,
                UserId = userId,
                UserName = userName,
                Browser = browser,
                Os = os,
                Device = device,
                BrowserInfo = browserInfo,
                ElapsedMs = elapsedMs
            };
        }
    }
}
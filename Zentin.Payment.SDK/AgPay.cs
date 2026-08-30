namespace Zentin.Payment.SDK
{
    public abstract class AgPay
    {
        public static readonly string LIVE_API_BASE = "https://pay.agpay.com";
        public static readonly string VERSION = "1.0";
        public static readonly string DEFAULT_SIGN_TYPE = "MD5";
        public static readonly string API_VERSION_NAME = "version";
        public static readonly string API_SIGN_TYPE_NAME = "signType";
        public static readonly string API_SIGN_NAME = "sign";
        public static readonly string API_REQ_TIME_NAME = "reqTime";

        /// <summary>
        /// default date format
        /// </summary>
        public static readonly string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// date default timezone
        /// </summary>
        public static readonly string DATE_TIMEZONE = "GMT+8";

        public static string AcceptLanguage { get; private set; } = "zh-CN";

        public static volatile string MchNo;

        public static volatile string AppId;

        /// <summary>
        /// apiKey
        /// </summary>
        public static volatile string ApiKey;

        /// <summary>
        /// api address
        /// </summary>
        public static string ApiBase { get; private set; } = LIVE_API_BASE;

        //public static volatile string privateKey;
        //public static volatile string privateKeyPath;

        //public static bool DEBUG = false;

        public static readonly int DEFAULT_CONNECT_TIMEOUT = 30 * 1000;
        public static readonly int DEFAULT_READ_TIMEOUT = 80 * 1000;

        private static volatile int ConnectTimeout = -1;

        private static volatile int ReadTimeout = -1;

        /// <summary>
        /// set the maximum number of retries when the connection fails
        /// </summary>
        private static volatile int MaxNetworkRetries = 1;

        public static void OverrideApiBase(string overriddenApiBase)
        {
            ApiBase = overriddenApiBase;
        }

        public static string GetApiBase()
        {
            return ApiBase;
        }

        public static void SetApiBase(string apiBase)
        {
            ApiBase = apiBase;
        }

        /// <summary>
        /// network connection timeout
        /// </summary>
        /// <returns></returns>
        public static int GetConnectTimeout()
        {
            if (ConnectTimeout == -1)
            {
                return DEFAULT_CONNECT_TIMEOUT;
            }
            return ConnectTimeout;
        }

        /// <summary>
        /// set network connection timeout (milliseconds)
        /// </summary>
        /// <param name="timeout"></param>
        public static void SetConnectTimeout(int timeout)
        {
            ConnectTimeout = timeout;
        }

        /// <summary>
        /// data read timeout
        /// </summary>
        /// <returns></returns>
        public static int GetReadTimeout()
        {
            if (ReadTimeout == -1)
            {
                return DEFAULT_READ_TIMEOUT;
            }
            return ReadTimeout;
        }

        /// <summary>
        /// set data read timeout (milliseconds)
        /// different interfaces have different time consumption, and some interfaces may take longer.
        /// </summary>
        /// <param name="timeout"></param>
        public static void SetReadTimeout(int timeout)
        {
            ReadTimeout = timeout;
        }

        /// <summary>
        /// get the maximum number of retries when the connection fails
        /// </summary>
        /// <returns></returns>
        public static int GetMaxNetworkRetries()
        {
            return MaxNetworkRetries;
        }

        /// <summary>
        ///  set the maximum number of retries when the connection fails
        /// </summary>
        /// <param name="numRetries"></param>
        public static void SetMaxNetworkRetries(int numRetries)
        {
            MaxNetworkRetries = numRetries;
        }

        public static string GetAcceptLanguage()
        {
            return AcceptLanguage;
        }

        public static void SetAcceptLanguage(string acceptLanguage)
        {
            AcceptLanguage = acceptLanguage;
        }
    }
}
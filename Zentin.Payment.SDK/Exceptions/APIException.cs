namespace Zentin.Payment.SDK.Exceptions
{
    /// <summary>
    /// API异常
    /// </summary>
    public class APIException : AgPayException
    {
        public APIException(int statusCode, string message, Exception innerException)
            : base(statusCode, message, innerException)
        {
        }
    }
}

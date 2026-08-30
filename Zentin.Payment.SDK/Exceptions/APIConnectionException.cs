namespace Zentin.Payment.SDK.Exceptions
{
    /// <summary>
    ///  api connection exception
    /// </summary>
    public class APIConnectionException : AgPayException
    {
        public APIConnectionException(string errorMessage)
            : base(errorMessage)
        {
        }

        public APIConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

namespace Zentin.Payment.Notification.Email
{
    public class EmailOptions
    {
        public const string SectionName = "Mail";

        /// <summary>
        /// same message sending interval
        /// </summary>
        public int? IntervalSeconds { get; set; }

        /// <summary>
        /// SMTP server address
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///  SMTP port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// sending name
        /// </summary>
        public string FromName { get; set; }

        /// <summary> 
        /// sending address
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// sender password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// recipient inbox
        /// </summary>
        public List<string> ToAddress { get; set; }
    }
}

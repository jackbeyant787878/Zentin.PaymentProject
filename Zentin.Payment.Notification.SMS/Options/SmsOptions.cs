namespace Zentin.Payment.Notification.SMS
{
    public class SmsOptions
    {
        public const string SectionName = "Sms";

        /// <summary>
        ///  same message sending interval
        /// </summary>
        public int? IntervalSeconds { get; set; }

        /// <summary>
        /// used sms service type：aliyunSms used
        /// </summary>
        public string SmsUseType { get; set; }

        public AliyunSmsOptions AliyunSms { get; set; }
    }

    public class AliyunSmsOptions
    {
        public const string SectionName = "AliyunSms";

        public string Endpoint { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string AccountOpenTemplateId { get; set; }
        public string ForgetPwdTemplateId { get; set; }
        public string LoginMchTemplateId { get; set; }
        public string MbrTelBindTemplateId { get; set; }
        public string RegisterMchTemplateId { get; set; }
        public string SignName { get; set; }
    }
}

using Zentin.Payment.Notification.Core;

namespace Zentin.Payment.Notification.SMS
{
    public class SmsSendRequest : NoticeSendRequest
    {
        public string PhoneNumbers { get; set; }
    }
}

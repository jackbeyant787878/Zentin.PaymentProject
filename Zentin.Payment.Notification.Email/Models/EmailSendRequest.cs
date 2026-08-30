using Zentin.Payment.Notification.Core;

namespace Zentin.Payment.Notification.Email
{
    public class EmailSendRequest : NoticeSendRequest
    {
        public List<string> ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public byte[] Attachments { get; set; }
        public string FileName { get; set; } = "Unnamed File.txt";
    }
}

using MimeKit;

namespace Zentin.Payment.Notification.Email
{
    internal class EmailHelper
    {
        public static MimeMessage CreateMimeMessage(EmailSendRequest input, EmailOptions option)
        {
            MimeMessage mimeMessage = new MimeMessage();
            var fromMailAddress = new MailboxAddress(option.FromName ?? option.FromAddress, option.FromAddress);
            mimeMessage.From.Add(fromMailAddress);
            foreach (var toMailAddress in (input.ToAddress ?? option.ToAddress).Select(MailboxAddress.Parse))
            {
                mimeMessage.To.Add(toMailAddress);
            }
            BodyBuilder bodyBuilder = new BodyBuilder()
            {
                HtmlBody = input.Body,
            };
            if (input.Attachments != null)
            {
                var attachment = bodyBuilder.Attachments.Add(input.FileName, input.Attachments);
                //Fix Chinese filename encoding issues
                const string charset = "GB18030";
                attachment.ContentType.Parameters.Clear();
                attachment.ContentDisposition.Parameters.Clear();
                attachment.ContentType.Parameters.Add(charset, "name", input.FileName);
                //Fix filename length issues
                foreach (var param in attachment.ContentType.Parameters)
                {
                    param.EncodingMethod = ParameterEncodingMethod.Rfc2047;
                }
            }
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = input.Subject;
            return mimeMessage;
        }

    }
}

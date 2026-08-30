using Zentin.Payment.Notification.Core;
using Zentin.Payment.Notification.Email;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailExtension
    {
        public static NoticeOptions UseEmail( this NoticeOptions options,Action<EmailOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            options.RegisterExtension(new EmailOptionsExtension(configure));
            return options;
        }
    }
}
using Zentin.Payment.Notification.Core;
using Zentin.Payment.Notification.SMS;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SmsExtension
    {
        public static NoticeOptions UseSms(this NoticeOptions options, Action<SmsOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            options.RegisterExtension(new SmsOptionsExtension(configure));
            return options;
        }
    }
}
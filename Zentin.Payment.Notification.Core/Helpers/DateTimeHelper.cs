namespace Zentin.Payment.Notification.Core
{
    public class DateTimeHelper
    {
        public static long GetTimestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}

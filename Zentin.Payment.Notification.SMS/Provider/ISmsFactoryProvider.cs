namespace Zentin.Payment.Notification.SMS
{
    public interface ISmsFactoryProvider
    {
        ISmsProvider GetProvider();
    }
}

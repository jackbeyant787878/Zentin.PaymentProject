namespace Zentin.Payment.Application.Contracts.IntegrationEvents
{
    public static class IntegrationEventTopics
    {
        public const string PayOrderPaid = "zentin.payorder.paid";
        public const string MchNotifyDispatch = "zentin.mchnotify.dispatch";
    }

    public sealed record PayOrderPaidIntegrationEvent(
        string PayOrderId,
        string MchNo,
        long Amount,
        DateTime OccurredOnUtc);

    /// <summary>
    /// Dispatch one merchant notify attempt; retry messages carry a CAP delay.
    /// </summary>
    public sealed record MchNotifyDispatchIntegrationEvent(long NotifyId);
}
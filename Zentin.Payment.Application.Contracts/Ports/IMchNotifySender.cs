namespace Zentin.Payment.Application.Contracts.Ports
{
    /// <summary>
    /// Outbound HTTP sender for merchant notifications.
    /// Returns the merchant response text, or an error description when the request fails;
    /// only "SUCCESS" (case-insensitive) terminates the retry loop.
    /// </summary>
    public interface IMchNotifySender
    {
        Task<string> SendAsync(string notifyUrl, string method, string mediaType, string? body, CancellationToken cancellationToken = default);
    }
}
namespace Zentin.Payment.Notification.Core
{
    public interface INotice
    {
        /// <summary>
        /// send notification
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<NoticeSendResponse> SendAsync(NoticeSendRequest request);
    }
}
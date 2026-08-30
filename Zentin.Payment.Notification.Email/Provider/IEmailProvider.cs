using Zentin.Payment.Notification.Core;

namespace Zentin.Payment.Notification.Email
{
    public interface IEmailProvider : INotice
    {
        /// <summary>
        ///  set the sending address
        /// </summary>
        /// <param name="toAddress"></param>
        public void SetToAddress(List<string> toAddress);

        /// <summary>
        /// push exception message
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="ex">exception</param>
        /// <returns></returns>
        Task<NoticeSendResponse> SendAsync(string title, Exception ex);

        /// <summary>
        ///  push normal message
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message content</param>
        /// <returns></returns>
        Task<NoticeSendResponse> SendAsync(string title, string message);
    }
}

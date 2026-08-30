using Microsoft.Extensions.Options;
using Zentin.Payment.Notification.Core;

namespace Zentin.Payment.Notification.SMS
{
    internal class AliyunSmsProvider : ISmsProvider
    {
        private readonly SmsOptions _smsOptions;
        private readonly NoticeOptions _noticeOptions;

        public AliyunSmsProvider(IOptions<SmsOptions> smsOptions, IOptions<NoticeOptions> noticeOptions)
        {
            _smsOptions = smsOptions.Value;
            _noticeOptions = noticeOptions.Value;
        }

        public Task<NoticeSendResponse> SendAsync(NoticeSendRequest request)
        {
            return SendBaseAsync((SmsSendRequest)request);
        }

        /// <summary>
        /// send message common method
        /// </summary>
        private async Task<NoticeSendResponse> SendBaseAsync(SmsSendRequest input)
        {
            var response = new NoticeSendResponse();
            try
            {
                await IntervalHelper.IntervalExcuteAsync(async () =>
                {
                    await Task.Delay(1000);
                }, input.PhoneNumbers, _smsOptions.IntervalSeconds ?? _noticeOptions.IntervalSeconds ?? 10);
            }
            catch (Exception e)
            {
                response.ErrMsg = $"SMS Send Exception: {e.Message}";
            }
            return response;
        }
    }
}

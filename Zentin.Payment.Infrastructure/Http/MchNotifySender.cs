using System.Text;
using Zentin.Payment.Application.Contracts.Ports;

namespace Zentin.Payment.Infrastructure.Http
{
    /// <summary>
    /// HTTP sender for merchant notifications. Donor uses a 20000-second timeout (bug);
    /// Zentin uses a 15-second named client. Non-SUCCESS responses and transport errors feed the retry loop.
    /// </summary>
    public sealed class MchNotifySender : IMchNotifySender
    {
        public const string ClientName = "mch-notify";

        private readonly IHttpClientFactory _httpClientFactory;

        public MchNotifySender(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendAsync(string notifyUrl, string method, string mediaType, string? body, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                HttpResponseMessage response;
                if ("GET".Equals(method, StringComparison.OrdinalIgnoreCase))
                {
                    response = await client.GetAsync(notifyUrl, cancellationToken);
                }
                else
                {
                    using var content = new StringContent(body ?? string.Empty, Encoding.UTF8, mediaType);
                    response = await client.PostAsync(notifyUrl, content, cancellationToken);
                }

                using (response)
                {
                    return await response.Content.ReadAsStringAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                // 供体语义：传输异常转为响应文本，进入重试判定（PayOrderMchNotifyMQReceiver.cs:88-92）
                return $"连接[{notifyUrl}]异常:【{e.Message}】";
            }
        }
    }
}
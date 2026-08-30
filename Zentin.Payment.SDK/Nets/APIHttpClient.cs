using System.Net;
using System.Text;
using Zentin.Payment.SDK.Exceptions;

namespace Zentin.Payment.SDK.Nets
{
    /// <summary>
    /// HTTP request client
    /// </summary>
    public class APIHttpClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<APIAgPayResponse> RequestAsync(APIAgPayRequest request)
        {
            int responseCode = 0;
            string responseBody = string.Empty;

            try
            {
                var httpRequest = new HttpRequestMessage(new HttpMethod(request.Method.ToString()), request.Url);

                using (var content = new StringContent(request.Content.Content, Encoding.GetEncoding(request.Content.CharSet), request.Content.MediaType))
                {
                    httpRequest.Content = content;

                    var response = await httpClient.SendAsync(httpRequest);

                    responseCode = (int)response.StatusCode;
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException e)
            {
                responseCode = (int)(e.StatusCode ?? HttpStatusCode.BadGateway);
                throw;
            }
            catch (Exception e)
            {
                throw new APIConnectionException($"Request AgPay ({request.Url}) exception, please check network or retry. Exception message: {e.Message}", e);
            }

            return new APIAgPayResponse(responseCode, responseBody, null);
        }

        /// <summary>
        /// Send HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public APIAgPayResponse Request(APIAgPayRequest request)
        {
            int responseCode = 0;
            string responseBody = string.Empty;

            try
            {
                var httpRequest = new HttpRequestMessage(new HttpMethod(request.Method.ToString()), request.Url);

                using (var content = new StringContent(request.Content.Content, Encoding.GetEncoding(request.Content.CharSet), request.Content.MediaType))
                {
                    httpRequest.Content = content;

                    var response = httpClient.SendAsync(httpRequest).Result;

                    responseCode = (int)response.StatusCode;
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException e)
            {
                responseCode = (int)(e.StatusCode ?? HttpStatusCode.BadGateway);
                throw;
            }
            catch (Exception e)
            {
                throw new APIConnectionException($"Request AgPay ({request.Url}) exception, please check network or retry. Exception message: {e.Message}", e);
            }

            return new APIAgPayResponse(responseCode, responseBody, null);
        }
    }
}
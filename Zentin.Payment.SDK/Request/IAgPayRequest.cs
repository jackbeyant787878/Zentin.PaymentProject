using Zentin.Payment.SDK.Models;
using Zentin.Payment.SDK.Nets;
using Zentin.Payment.SDK.Response;

namespace Zentin.Payment.SDK.Request
{
    /// <summary>
    /// AgPay request interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAgPayRequest<T> where T : AgPayResponse
    {
        /// <summary>
        /// Get the path of the current interface
        /// </summary>
        /// <returns></returns>
        string GetApiUri();

        /// <summary>
        /// Get the version of the current interface
        /// </summary>
        /// <returns></returns>
        string GetApiVersion();

        /// <summary>
        /// Set the version of the current interface
        /// </summary>
        /// <param name="apiVersion"></param>
        void SetApiVersion(string apiVersion);

        RequestOptions GetRequestOptions();

        void SetRequestOptions(RequestOptions options);

        AgPayObject GetBizModel();

        void SetBizModel(AgPayObject bizModel);
    }
}
using Zentin.Payment.SDK.Models;
using Zentin.Payment.SDK.Nets;
using Zentin.Payment.SDK.Response;

namespace Zentin.Payment.SDK.Request
{
    /// <summary>
    ///  payment order query request implementation
    /// </summary>
    public class PayOrderQueryRequest : IAgPayRequest<PayOrderQueryResponse>
    {
        private string ApiVersion = AgPay.VERSION;
        private readonly string ApiUri = "api/pay/query";
        private RequestOptions Options;
        private AgPayObject BizModel = null;

        public string GetApiUri()
        {
            return this.ApiUri;
        }

        public string GetApiVersion()
        {
            return this.ApiVersion;
        }

        public void SetApiVersion(string apiVersion)
        {
            this.ApiVersion = apiVersion;
        }

        public RequestOptions GetRequestOptions()
        {
            return this.Options;
        }

        public void SetRequestOptions(RequestOptions options)
        {
            this.Options = options;
        }

        public AgPayObject GetBizModel()
        {
            return this.BizModel;
        }

        public void SetBizModel(AgPayObject bizModel)
        {
            this.BizModel = bizModel;
        }
    }
}

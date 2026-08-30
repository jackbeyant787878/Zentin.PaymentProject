

using Zentin.Payment.Application.Contracts.Queries.Abstractions;

namespace Zentin.Payment.Application.Dto
{
    /// <summary>
    /// 统计查询
    /// </summary>
    public class StatisticQueryDto : DatePagedQuery
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchNo { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MchName { get; set; }

        /// <summary>
        /// 代理商号
        /// </summary>
        public string AgentNo { get; set; }

        /// <summary>
        /// 代理商名称
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// 服务商号
        /// </summary>
        public string IsvNo { get; set; }

        /// <summary>
        /// 服务商名称
        /// </summary>
        public string IsvName { get; set; }

        /// <summary>
        /// 通道代码
        /// </summary>
        public string IfCode { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string IfName { get; set; }

        /// <summary>
        /// 门店ID
        /// </summary>
        public long? StoreId { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 支付方式代码
        /// </summary>
        public string WayCode { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string WayName { get; set; }

        /// <summary>
        /// 支付类型: WECHAT-微信, ALIPAY-支付宝, YSFPAY-云闪付, UNIONPAY-银联, DCEPPAY-数字人民币, OTHER-其他
        /// </summary>
        public string WayType { get; set; }

        public string QueryDateType { get; set; }

        public string Method { get; set; }

        public string Format { get; set; } = "yyyy-MM-dd";

        public override void BindDateRange()
        {
            base.BindDateRange();

            switch (QueryDateType)
            {
                case StatisticConstants.QueryDateType.DAY:
                    Format = "yyyy-MM-dd";
                    CreatedStart ??= DateTime.Today.AddMonths(-1);
                    CreatedEnd ??= DateTime.Today.AddSeconds(-1);
                    break;
                case StatisticConstants.QueryDateType.MONTH:
                    Format = "yyyy-MM";
                    CreatedStart ??= DateTime.Today.AddYears(-1);
                    CreatedEnd ??= DateTime.Today.AddSeconds(-1);
                    break;
                case StatisticConstants.QueryDateType.YEAR:
                    Format = "yyyy";
                    CreatedStart ??= DateTime.Today.AddYears(-1);
                    CreatedEnd ??= DateTime.Today.AddSeconds(-1);
                    break;
                default:
                    break;
            }
        }
    }

    public class StatisticConstants
    {
        public interface Method
        {
            public const string TRANSACTION = "transaction";
            public const string MCH = "mch";
            public const string STORE = "store";
            public const string WAY_CODE = "wayCode";
            public const string WAY_TYPE = "wayType";
            public const string AGENT = "agent";
            public const string ISV = "isv";
            public const string CHANNEL = "channel";
        }

        public interface QueryDateType
        {
            public const string DAY = "day";
            public const string MONTH = "month";
            public const string YEAR = "year";
        }
    }
}

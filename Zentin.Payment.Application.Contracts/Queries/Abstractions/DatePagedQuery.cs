using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.Application.Contracts.Queries.Abstractions
{

    public abstract class DatePagedQuery : PagedQuery
    {

        public virtual void BindDateRange()
        {
            QueryDateRange ??= string.Empty;
            DateUtil.GetQueryDateRange(QueryDateRange, out string createdStart, out string createdEnd);
            CreatedStart = string.IsNullOrWhiteSpace(createdStart) ? CreatedStart : Convert.ToDateTime(createdStart);
            CreatedEnd = string.IsNullOrWhiteSpace(createdEnd) ? CreatedEnd : Convert.ToDateTime(createdEnd);
        }
        public string QueryDateRange { get; set; } = string.Empty;
        public DateTime? CreatedStart { get; set; }

        public DateTime? CreatedEnd { get; set; }
    }
}

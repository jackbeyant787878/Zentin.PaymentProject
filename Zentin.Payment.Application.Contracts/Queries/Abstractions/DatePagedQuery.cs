using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Application.Contracts.Queries.Abstractions
{
   
    public abstract class DatePagedQuery : PagedQuery
    {
       
        public DateTime? CreatedStart { get; set; }

        public DateTime? CreatedEnd { get; set; }
    }
}

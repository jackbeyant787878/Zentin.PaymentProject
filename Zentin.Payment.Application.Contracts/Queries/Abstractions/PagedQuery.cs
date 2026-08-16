using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Application.Contracts.Queries.Abstractions
{
   
    public abstract class PagedQuery
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 20;

     
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value switch
            {
                -1 => int.MaxValue,
                > MaxPageSize => MaxPageSize,
                <= 0 => 20,
                _ => value
            };
        }
   
        public string? SortField { get; set; }

        public string? SortOrder { get; set; }
    }
}

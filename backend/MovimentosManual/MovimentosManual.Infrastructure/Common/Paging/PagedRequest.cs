// PagedRequest.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MovimentosManual.Infrastructure.Common.Paging
{
    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedRequestWithSort<T> : PagedRequest
    {
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public bool Descending { get; set; } = false;

        public T? Filters { get; set; } // ✅ Adicione isso
    }

    public class PagedQuery<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public Expression<Func<T, bool>>? Filter { get; set; }
        public List<OrderClause<T>> Orderings { get; set; } = new();

        public int Skip => (Page - 1) * PageSize;
    }

    public class OrderClause<T>
    {
        public Expression<Func<T, object>> Expression { get; set; }
        public bool Descending { get; set; } = false;

        public OrderClause(Expression<Func<T, object>> expression, bool descending = false)
        {
            Expression = expression;
            Descending = descending;
        }
    }

    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

        public IEnumerable<T> Items { get; set; } = new List<T>();

        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

        public PagedResult() { }

        public PagedResult(IEnumerable<T> items, int count, int page, int pageSize)
        {
            Items = items;
            TotalCount = count;
            Page = page;
            PageSize = pageSize;
        }
    }

}

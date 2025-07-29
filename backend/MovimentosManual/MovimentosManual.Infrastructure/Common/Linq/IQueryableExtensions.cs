using System;
using System.Linq;
using System.Linq.Expressions;
using MovimentosManual.Infrastructure.Common.Paging;

namespace MovimentosManual.Infrastructure.Common.Linq
{
        public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyPagedQuery<T>(this IQueryable<T> query, PagedQuery<T> pagedQuery)
        {
            if (pagedQuery.Filter != null)
            {
                query = query.Where(pagedQuery.Filter);
            }

            if (pagedQuery.Orderings is { Count: > 0 })
            {
                var first = pagedQuery.Orderings.First();
                var orderedQuery = first.Descending
                    ? query.OrderByDescending(first.Expression)
                    : query.OrderBy(first.Expression);

                foreach (var ordering in pagedQuery.Orderings.Skip(1))
                {
                    orderedQuery = ordering.Descending
                        ? orderedQuery.ThenByDescending(ordering.Expression)
                        : orderedQuery.ThenBy(ordering.Expression);
                }

                query = orderedQuery;
            }

            return query
                .Skip(pagedQuery.Skip)
                .Take(pagedQuery.PageSize);
        }
    }
}

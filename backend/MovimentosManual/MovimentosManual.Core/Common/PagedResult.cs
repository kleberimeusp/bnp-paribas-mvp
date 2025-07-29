namespace MovimentosManual.Core.Common
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public  int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }

        public static PagedResult<T> Create(IEnumerable<T> source, int page, int pageSize)
        {
            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                TotalItems = source.Count(),
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }
    }
}

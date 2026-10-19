namespace ProductAPI.DTOs
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } =
            Array.Empty<T>();

        public PaginationMeta Meta { get; set; } = new();
    }

    public class PaginationMeta
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
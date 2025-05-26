namespace PSK2025.Models.DTOs
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
        public int? TotalPages => PageSize != null ? (int)Math.Ceiling(TotalCount / (double)PageSize) : null;
        public bool HasPrevious => CurrentPage != null && CurrentPage > 1;
        public bool HasNext => TotalPages != null && CurrentPage < TotalPages;
    }
}
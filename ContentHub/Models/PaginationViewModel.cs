namespace ContentHub.Models
{
    public class PaginationViewModel
    {
        public List<WordCounts>? Words { get; set; }
        public int TotalWordCounts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
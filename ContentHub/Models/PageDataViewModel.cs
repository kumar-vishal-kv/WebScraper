namespace ContentHub.Models
{
    public class PageDataViewModel
    {
        public string? WebsiteUrl { get; set; }

        public DateTime? LastUpdated { get; set; }

        public List<ImageAttributes> ImageAttributes { get; set; }

        public List<WordCounts> WordCounts { get; set; }

        public PageDataViewModel()
        {
            ImageAttributes = new List<ImageAttributes>();

            WordCounts = new List<WordCounts>();
        }
    }
}
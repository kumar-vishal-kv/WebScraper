namespace ContentHub.Models
{
    public class DisplayViewModel
    {
        public List<ImageAttributes> Images { get; set; }
        public Dictionary<string, int> WordCounts { get; set; }

        public DisplayViewModel()
        {
            Images = new List<ImageAttributes>();
            WordCounts = new Dictionary<string, int>();
        }
    }
}
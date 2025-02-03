using System.Security.Cryptography.X509Certificates;

namespace ContentHub.Models
{
    public class WordCounts
    {
        public string Word { get; set; }
        public int Count { get; set; }
    
        //Ensuring Worcounts in never null
        public WordCounts(string word = "", int count = 0) 
        {
            Word = word ?? throw new ArgumentNullException(nameof(word)); 
            Count = count;
        }
    }
}
using ContentHub.Models;

namespace ContentHub.Repository.Content
{
    public interface IContentRepository
    {
        //List<WordCounts> GetTop10Words(string content);
        public Dictionary<string, int> GetAllWordsCount(string content);
    }
}
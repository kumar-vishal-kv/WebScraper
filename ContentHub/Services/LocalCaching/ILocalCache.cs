using ContentHub.Models;

namespace ContentHub.Services.Local
{
    public interface ILocalCache
    {
        (List<WordCounts>, List<ImageAttributes>) GetContentFromLocalCache(string url, DateTime? dateTime);
        (List<WordCounts>, List<ImageAttributes>) GetContentFromLocalCache(string url);
        bool CheckLocalCache(string url, DateTime? dateTime);
        bool CheckLocalCache(string url);
        bool AddOrUpdateLocalCache(PageDataViewModel pageData);
    }
}
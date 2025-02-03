using ContentHub.Models;

namespace ContentHub.Services.ContentFetcher
{
    public interface IFetchWebsiteContent
    {
        Task<string> FetchContentFromUrl(string url);
    }
}
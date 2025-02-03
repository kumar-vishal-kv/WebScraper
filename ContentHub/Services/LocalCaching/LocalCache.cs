using ContentHub.Models;
using Newtonsoft.Json;

namespace ContentHub.Services.Local
{
    public class LocalCache : ILocalCache
    {
        private readonly ILogger<LocalCache> _logger;
        public LocalCache(ILogger<LocalCache> logger) 
        {
            _logger = logger;
        }
        private static Dictionary<string, PageDataViewModel> pageDataDictionary = new Dictionary<string, PageDataViewModel>();

        ////Funciton to Check Local Cache based on url and last modified date
        public bool CheckLocalCache(string url, DateTime? dateTime)
        {
            if (pageDataDictionary.ContainsKey(url) && pageDataDictionary[url].LastUpdated == dateTime)
            {
                return true;
            }

            return false;
        }

        //Funciton to Check Local Cache based on url only
        public bool CheckLocalCache(string url)
        {
            if (pageDataDictionary.ContainsKey(url))
            {
                return true;
            }

            return false;
        }

        //Function to get content from Local Cache based on url and lastupdated date
        public (List<WordCounts>, List<ImageAttributes>) GetContentFromLocalCache(string url, DateTime? dateTime)
        {   
            //Fetch Data from Local Cache
            List<WordCounts> wordCounts = pageDataDictionary[url].WordCounts;
            List<ImageAttributes> imageAttributes = pageDataDictionary[url].ImageAttributes;
            
            _logger.LogInformation($"Contents fetched from local cahe : WordCount = {wordCounts.Count}, ImageCount ={imageAttributes.Count}, Last Updated Website Date = {dateTime}");

            return (wordCounts, imageAttributes);                        
        }

        //Function to get content from Local Cache based on url only
        public (List<WordCounts>, List<ImageAttributes>) GetContentFromLocalCache(string url)
        {
            //Fetch Data from Local Cache
            List<WordCounts> wordCounts = pageDataDictionary[url].WordCounts;
            List<ImageAttributes> imageAttributes = pageDataDictionary[url].ImageAttributes;

            _logger.LogInformation($"Contents fetched from local cahe : WordCount = {wordCounts.Count}, ImageCount ={imageAttributes.Count}");

            return (wordCounts, imageAttributes);    
        }

        //Function to update/add Local Cache
        public bool AddOrUpdateLocalCache(PageDataViewModel pageData)
        {
            if (pageData != null && pageData.WebsiteUrl != null)
            {
                //Sort data before populating
                pageData.WordCounts = pageData.WordCounts.OrderByDescending(word => word.Count).ToList();

                //Check if the url exists in the pageDataDictionary with specific date or not
                if (pageDataDictionary.ContainsKey(pageData.WebsiteUrl) &&
                    ((pageDataDictionary[pageData.WebsiteUrl].LastUpdated != pageData.LastUpdated) || (pageDataDictionary[pageData.WebsiteUrl].LastUpdated == null)))
                {
                    pageDataDictionary[pageData.WebsiteUrl] = pageData;
                    _logger.LogInformation($"The following url \"{pageData.WebsiteUrl}\" is updated in the local cache");
                }
                else if (!pageDataDictionary.ContainsKey(pageData.WebsiteUrl))
                {
                    pageDataDictionary[pageData.WebsiteUrl] = pageData;
                    _logger.LogInformation($"The following url \"{pageData.WebsiteUrl}\" is added to the local cache");
                }
                else
                {
                    _logger.LogInformation($"The following url \"{pageData.WebsiteUrl}\" already exists in the local cache");
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}
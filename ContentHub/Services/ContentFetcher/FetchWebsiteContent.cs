using Microsoft.Extensions.ObjectPool;
using System.Collections.Specialized;
using System.Text;

namespace ContentHub.Services.ContentFetcher
{
    public class FetchWebsiteContent : IFetchWebsiteContent
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FetchWebsiteContent> _logger;

        public FetchWebsiteContent(HttpClient httpClient, ILogger<FetchWebsiteContent> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> FetchContentFromUrl(string url)
        {
            try
            {
                //Set up Http connection with the client
                var response = await _httpClient.GetAsync(url);
                _logger.LogInformation("Logging Response {0}", response.StatusCode);
                response.EnsureSuccessStatusCode();

                //Get Content as String from url
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching the URL: {0}", ex.Message);
                return string.Empty;
            }


            //try
            //{
            //    try
            //    {
            //        if(GetLastModifiedDate(url) != null)
            //        {

            //        }
            //    }
            //    catch
            //    {

            //    }


            //}
            //catch (Exception)
            //{
            //    throw new Exception("Failed to fetch content");
            //}            
        }

        //static async Task<DateTime?> GetLastModifiedDate(string url)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        //        if (response.Headers.Contains("Last-Modified"))
        //        {
        //            string lastModifiedStr = response.Headers.GetValues("Last-Modified").ToString();
        //            return DateTime.Parse(lastModifiedStr);
        //        }
        //        return null;
        //    }
        //}
    }
}
using ContentHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContentHub.Repository.Image;
using ContentHub.Repository.Content;
using ContentHub.Services.Local;
using ContentHub.Services.ContentFetcher;


namespace ContentHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly HttpClient _httpClient;
        private readonly IFetchWebsiteContent _websiteContentFetcher;
        private readonly IImageRepository _imageRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILocalCache _localCache;

        //Injecting different services
        public HomeController(ILogger<HomeController> logger, IFetchWebsiteContent websiteContentFetcher,
            IImageRepository imageRepository, IContentRepository contentRepository, ILocalCache localCache)
        {
            _logger = logger;
            //_httpClient = httpClient;
            _websiteContentFetcher = websiteContentFetcher;
            _imageRepository = imageRepository;
            _contentRepository = contentRepository;
            _localCache = localCache;
        }

        public IActionResult Index(bool? returnedFromErrorPage)
        {
            ViewData["Title"] = "Home";

            if (returnedFromErrorPage == null)
            {
                _logger.LogInformation("Returned From Error Page : Welcome to Home Page");
            }
            else
            {
                _logger.LogInformation("Welcome to Home Page");
            }
                        
            return View();
        }

        //Analyzing Url
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalyzeUrl(string url)
        {
            ViewData["Title"] = "Results";
            // First validate the URL format
            var isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!isValidUrl)
            {
                ModelState.AddModelError("url", "The URL is not valid. Please provide a valid HTTP or HTTPS URL.");
                _logger.LogError($"The {url} is not valid. Please provide a valid HTTP or HTTPS URL.");
                return View("Error");
            }

            // Check if the URL is reachable
            var isReachable = await CheckUrlAvailability(url);
            if (!isReachable)
            {
                ModelState.AddModelError("url", "The URL is not reachable. Please check the URL and try again.");
                _logger.LogError($"The ${url} is not reachable. Please check the URL and try again.");
                return View("Error");
            }

            try
            {
                var images = new List<ImageAttributes>();
                var wordCount = new Dictionary<string, int>();
                if(_localCache.CheckLocalCache(url))
                {
                    _logger.LogInformation("URL present in local cache");
                    try
                    {
                        (var wordCounts, images) = _localCache.GetContentFromLocalCache(url);
                        wordCount = wordCounts.Where(w => w.Word != null).ToDictionary(w => w.Word!, w => w.Count);

                        _logger.LogInformation("Word Count and Images fetched from cache");
                    }
                    catch (Exception)
                    {
                        _logger.LogError("Error in fetching content from Local cache");
                        return View("Error");
                    }
                }
                else 
                {
                    _logger.LogInformation($"Fetching content from - {url}");
                    // Fetch the HTML content from the URL
                    var htmlContent = await _websiteContentFetcher.FetchContentFromUrl(url);

                    // Extract images from the raw HTML content
                    images = _imageRepository.GetImagesUrls(htmlContent, url);

                    // Extract word count from the raw HTML content
                    wordCount = _contentRepository.GetAllWordsCount(htmlContent).OrderByDescending(wc => wc.Value)
                        .ToDictionary(wc => wc.Key, wc => wc.Value);                      
                    
                    _logger.LogInformation($"Words = {wordCount.Count} and images = {images.Count}");

                    if (!wordCount.Any() && !images.Any())
                    {
                        ModelState.AddModelError("url", "The page is Empty or built using JS framework");
                        _logger.LogError("The page is Empty or built using JS framework");
                        return View("Error");
                    }

                    var changeToList = wordCount.Select(kvp => new WordCounts
                    {
                        Word = kvp.Key,
                        Count = kvp.Value
                    }).ToList();


                    _localCache.AddOrUpdateLocalCache(new PageDataViewModel() { WebsiteUrl = url, ImageAttributes = images, WordCounts = changeToList });                    

                }

                var model = new DisplayViewModel
                {
                    Images = images,
                    WordCounts = wordCount
                };
                return View("Result", model);
            }
            catch (Exception)
            {
                _logger.LogError("Error fetching content from {0}", url);
                return View("Error");
            }
        }

        public async Task<bool> CheckUrlAvailability(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    _logger.LogInformation($"Response : {response}. Success Response");
                    return response.IsSuccessStatusCode; // it returns true if response id between 200-299
                }
                catch (HttpRequestException)
                {
                    _logger.LogError($"Response other than between 200-299");
                    return false; // it returns false in other cases
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
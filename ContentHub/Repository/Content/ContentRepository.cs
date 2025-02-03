using ContentHub.Services.ContentFetcher;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web; // Add this namespace for HtmlDecode

namespace ContentHub.Repository.Content
{
    public class ContentRepository : IContentRepository
    {
        private readonly ILogger<ContentRepository> _logger;

        public ContentRepository(ILogger<ContentRepository> logger) 
        {
            _logger = logger; 
        }
        public Dictionary<string, int> GetAllWordsCount(string content)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            //Select the body node for content
            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

            //Check body node is present or not
            if (bodyNode == null)
                return new Dictionary<string, int>(); 

            //Extract text from body tag
            var textContent = bodyNode
                .DescendantsAndSelf()
                .Where(n => n.NodeType == HtmlNodeType.Text &&
                            !n.ParentNode.Name.Equals("script", StringComparison.OrdinalIgnoreCase) &&
                            !n.ParentNode.Name.Equals("style", StringComparison.OrdinalIgnoreCase) &&
                            !n.ParentNode.Name.Equals("code", StringComparison.OrdinalIgnoreCase) &&
                            !n.ParentNode.Name.Equals("i", StringComparison.OrdinalIgnoreCase) &&
                            !n.ParentNode.Name.Equals("b", StringComparison.OrdinalIgnoreCase) &&
                            !n.ParentNode.Name.Equals("u", StringComparison.OrdinalIgnoreCase))
                .Aggregate("", (current, node) => current + " " + node.InnerText);

            //Decoding html entity reference to normal text
            textContent = HttpUtility.HtmlDecode(textContent);

            if(String.IsNullOrWhiteSpace(textContent))
            {
                _logger.LogError("Page might be built on JS framework or mightn't have content");
                return new Dictionary<string, int>();
            }

            //Cleaning text and extract words
            textContent = textContent.Replace("<br>", "\n");
            var cleanedText = Regex.Replace(textContent, @"[^a-zA-Z\s]", "").ToLower();
            cleanedText = Regex.Replace(cleanedText, @"\s+", " ").Trim();
            var words = cleanedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Populating dictionary with words and count
            var wordDictionary = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordDictionary.ContainsKey(word))
                {
                    wordDictionary[word]++;
                }
                else
                {
                    wordDictionary[word] = 1;
                }
            }
            return wordDictionary;
        }
    }
}
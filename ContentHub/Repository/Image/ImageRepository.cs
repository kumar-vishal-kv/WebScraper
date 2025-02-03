using ContentHub.Models;
using HtmlAgilityPack;

namespace ContentHub.Repository.Image
{
    public class ImageRepository : IImageRepository
    {

        public List<ImageAttributes> GetImagesUrls(string content, string baseUrl)
        {
            var images = new List<ImageAttributes>();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                //Get the img tags/nodes
                var imgNodes = doc.DocumentNode.SelectNodes("//img");
                if (imgNodes != null)
                {
                    foreach (var imgNode in imgNodes)
                    {
                        var src = imgNode.GetAttributeValue("src", string.Empty);
                        var altText = imgNode.GetAttributeValue("alt", String.Empty);
                        var fullUrl = string.Empty;

                        //Check if src has a value
                        if (!string.IsNullOrEmpty(src))
                        {
                            fullUrl = GetFullUrl(src, baseUrl);
                            images.Add(new ImageAttributes { AltText = altText, Url = fullUrl });
                        }

                        //Check if alternate text is present
                        if (string.IsNullOrEmpty(altText))
                        {
                            Uri uri = new Uri(fullUrl);
                            altText = Path.GetFileName(uri.LocalPath);
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed");
            }
            return images;
        }

        private static string GetFullUrl(string relativeUrl, string baseUrl)
        {
            //If src is absolute URL
            if (Uri.IsWellFormedUriString(relativeUrl, UriKind.Absolute))
            {
                return relativeUrl;
            }

            // Is src is relative path
            var baseUri = new Uri(baseUrl);
            var fullUri = new Uri(baseUri, relativeUrl);
            return fullUri.ToString();
        }

    }
}
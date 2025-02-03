using ContentHub.Models;

namespace ContentHub.Repository.Image
{
    public interface IImageRepository
    {
        List<ImageAttributes> GetImagesUrls(string content, string url);
    }
}
using Microsoft.AspNetCore.Http;

namespace pv311_web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string directoryPath);
        void DeleteImage(string filePath);
    }
}

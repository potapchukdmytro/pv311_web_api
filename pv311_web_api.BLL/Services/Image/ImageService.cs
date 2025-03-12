using Microsoft.AspNetCore.Http;

namespace pv311_web_api.BLL.Services.Image
{
    public class ImageService : IImageService
    {
        public void DeleteImage(string filePath)
        {
            try
            {
                string rootPath = Path.Combine(Settings.FilesRootPath, "wwwroot", Settings.ImagesPath);
                string path = Path.Combine(rootPath, filePath);

                if(File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string?> SaveImageAsync(IFormFile image, string directoryPath)
        {
            try
            {
                var types = image.ContentType.Split("/");
                if (types[0] != "image")
                {
                    return null;
                }

                string rootPath = Path.Combine(Settings.FilesRootPath, "wwwroot", Settings.ImagesPath);
                string workPath = Path.Combine(rootPath, directoryPath);
                string imageName = $"{Guid.NewGuid()}.{types[1]}";
                string imagePath = Path.Combine(workPath, imageName);

                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }

                using (var stream = File.Create(imagePath))
                {
                    await image.CopyToAsync(stream);
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
            
            
        }
    }
}

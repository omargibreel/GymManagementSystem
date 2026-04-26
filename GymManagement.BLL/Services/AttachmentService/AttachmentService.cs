using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace GymManagement.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IWebHostEnvironment _webHost;
        public AttachmentService(IWebHostEnvironment webHostEnvironment)
        {
            _webHost = webHostEnvironment;
        }
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024;
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (file is null || folderName is null || file.Length == 0)
                    return null;

                if (file.Length > maxFileSize)
                    return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                file.CopyTo(fileStream);
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder = {folderName} : {ex}");
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;

                var fullBath = Path.Combine(_webHost.WebRootPath, "images", folderName, fileName);
                if (File.Exists(fullBath)) { 
                    File.Delete(fullBath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To Delete File With Name {fileName} : {ex}");
                return false;
            }
        }
    }
}

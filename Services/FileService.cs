using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace soccer.Services
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile file, string folder = "uploads");
    }

    public class FileService : IFileService
    {
        private readonly IHostEnvironment _env;
        public FileService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folder = "uploads")
        {
            if (file == null || file.Length == 0) return null;

            var uploads = Path.Combine(_env.ContentRootPath, "wwwroot", folder);
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folder}/{fileName}";
        }
    }
}

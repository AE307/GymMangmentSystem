using GymMangmentBLL.Service.Interfaces.AttachmentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedextension = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSize = 5*1025*1024;//5MB
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file is null || file.Length == 0) return null;
                if (file.Length > MaxFileSize) return null;
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedextension.Contains(extension)) return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                //wwwroot/images/members/dhduldjjld.png
                var filePath = Path.Combine(folderPath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                return fileName;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"failed to upload file to folder ={folderName}:{ex}");
                return null;
            }
        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;
                var fullPath = Path.Combine(_webHost.WebRootPath, "images", folderName, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to delete file with name={fileName}:{ex}");
                return false;
            }
        }
    }
}

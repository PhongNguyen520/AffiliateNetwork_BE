using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Services
{
    public class HepperUploadImage : IHepperUploadImage
    {
        private readonly Cloudinary _cloudinary;

        public HepperUploadImage(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new Exception("File ảnh không hợp lệ!");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = $"uploads/{Guid.NewGuid()}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // LOG RESPONSE ĐỂ XEM LỖI
            Console.WriteLine($"Upload Response: {Newtonsoft.Json.JsonConvert.SerializeObject(uploadResult)}");

            if (uploadResult == null || uploadResult.SecureUrl == null)
            {
                throw new Exception("Upload thất bại, kiểm tra lại API Key hoặc file upload!");
            }

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}

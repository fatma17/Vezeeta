using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;

namespace Vezeeta.Serivce
{
    public static class ImagesService
    {
        public static string Add(IFormFile Image)
        {
            //string uploadsfile = Path.Combine(_environment.WebRootPath, "Images");
            if (Image.FileName == null)
            {
                return ("Image not found");
            }
            string uniquefilaname = Guid.NewGuid().ToString() + "_" +Image.FileName;
            string imagepath = Path.Combine("wwwroot", "Images", uniquefilaname);

            //string imagepath = Path.Combine(uploadsfile, uniquefilaname);

            using (var fileStream = new FileStream(imagepath, FileMode.Create))
            {
                Image.CopyTo(fileStream);
                fileStream.Close();
            }
            return uniquefilaname;
        }
        public static string delete(string imageName)
        {
            if (imageName==null)
            {
                return ("Image not found");
            }

            string imagePath = Path.Combine("wwwroot", "images", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return ("Image not found");
            }
            System.IO.File.Delete(imagePath);

            return ($"Image '{imageName}' has been deleted");
        }
        public static string get(string ImageName)
        {
            string imagePath = Path.Combine("wwwroot", "Images", ImageName);
            if (!System.IO.File.Exists(imagePath))
            {
                return("Image not found");
            }
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

    }
}

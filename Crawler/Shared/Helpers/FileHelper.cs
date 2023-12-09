using System;
using System.IO;

namespace Crawler.Services.Helpers
{
    public class FileHelper
    {
        public static string FileToBase64String(string path)
        {
            try
            {
                using var image = new FileStream(path, FileMode.Open);
                using var memoryStream = new MemoryStream();

                image.CopyTo(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
            catch
            {
                return null;
            }
        }
    }
}

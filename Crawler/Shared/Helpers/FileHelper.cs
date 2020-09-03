using System;
using System.IO;

namespace Shared.Helpers
{
    public class FileHelper
    {
        public static string FileToBase64String(string path)
        {
            try
            {
                using var image = new FileStream(path, FileMode.Open);
                using var m = new MemoryStream();

                image.CopyTo(m);
                var imageBytes = m.ToArray();
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

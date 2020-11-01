using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.MVC.Middlewares
{
    public class ImageCachingOptions
    {
        public const string DefaultFolder = "images";

        public ImageCachingOptions()
            : this("images", 50, new TimeSpan(0, 20, 0))
        { }

        public ImageCachingOptions(string folder, short maxCount, TimeSpan expirationTime)
        {
            Path = folder ?? DefaultFolder;
            MaxCount = maxCount;
            ExpirationTime = expirationTime;
        }

        public string Path { get; set; }
        public int MaxCount { get; set; }
        public TimeSpan ExpirationTime { get; set; }
    }
}

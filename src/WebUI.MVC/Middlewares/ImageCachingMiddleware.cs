using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.MVC.Middlewares
{
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ImageCachingOptions _options;
        private readonly string _storageDirectory;

        public ImageCachingMiddleware(RequestDelegate next)
            : this(next, new ImageCachingOptions())
        { }

        public ImageCachingMiddleware(RequestDelegate next, ImageCachingOptions options)
        {
            _next = next;
            _options = options;
            _storageDirectory = Path.Combine(AppContext.BaseDirectory, _options.Path);
        }

        public async Task Invoke(HttpContext context)

        {
            if (context.Request.Path.ToString().Contains("/images/")) {
                context.Response.ContentType = "image/bmp";
                if (IsAvailableToGetFromLocalCache(context)) {
                    await GetFromDirectoryAsync(context);
                } else {

                    var imagePath = FullImagePath(context);

                    var originalBody = context.Response.Body;

                    using (var memoryStream = new MemoryStream()) {
                        context.Response.Body = memoryStream;

                        await _next.Invoke(context);

                        memoryStream.Position = 0;

                        SaveImageLocally(imagePath, memoryStream);

                        memoryStream.Position = 0;
                        await memoryStream.CopyToAsync(originalBody);
                    }
                }

                return;
            }

            await _next.Invoke(context);
        }


        public bool IsAvailableToGetFromLocalCache(HttpContext context)
        {
            if (context.Response.ContentType != "image/bmp") return false;

            var pathToStoreImages = Path.Combine(AppContext.BaseDirectory, _options.Path);
            if (!Directory.Exists(pathToStoreImages)) {
                Directory.CreateDirectory(pathToStoreImages);
            }

            if (Directory.GetFiles(pathToStoreImages).Length >= _options.MaxCount) {
                return false;
            }

            var imagePath = FullImagePath(context);
            var isImageExists = File.Exists(imagePath);

            var isExpired = DateTime.Now - File.GetLastWriteTime(imagePath) > _options.ExpirationTime;


            return isImageExists && !isExpired;
        }

        public void SaveImageLocally(string path, MemoryStream stream)
        {
            var pathToStoreImages = Path.Combine(AppContext.BaseDirectory, _options.Path);
            if (!Directory.Exists(pathToStoreImages)) {
                Directory.CreateDirectory(pathToStoreImages);
            } else if (Directory.GetCreationTime(pathToStoreImages) <= DateTime.Now.AddDays(_options.DaysForFullCleanUp)) {
                Directory.Delete(pathToStoreImages, true);
                Directory.CreateDirectory(pathToStoreImages);
            }

            using (var fileStream = File.OpenWrite(path)) {
                stream.CopyTo(fileStream);
            }
        }

        public async Task GetFromDirectoryAsync(HttpContext httpContext)
        {
            var imagePath = FullImagePath(httpContext);
            using (var fileStream = File.OpenRead(imagePath)) {
                await fileStream.CopyToAsync(httpContext.Response.Body);
            }
        }

        private string FullImagePath(HttpContext context)
        {
            var imageId = context.Request.Path.ToString().Split('/').Last();

            var imagePath = Path.Combine(_storageDirectory, $"{imageId}.bmp");
            return imagePath;
        }
    }
}
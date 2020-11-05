using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using WebUI.MVC.Middlewares;
using Xunit;

namespace WebUI.MVC.Tests.MiddlewaresTests
{
    public class ImageCachingMiddlewareTests : IDisposable
    {
        private readonly string _pathToImageFolder;

        public ImageCachingMiddlewareTests()
        {
            _pathToImageFolder = Path.Combine(AppContext.BaseDirectory, "images");

        }

        [Fact]
        public async Task ImageCachingMiddlewareTest_RequestContainsImagesUsingHosting_ReturnsContentTypeImageBmp()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                           
                        })
                        .Configure(app =>
                        {
                            app.UseMiddleware<ImageCachingMiddleware>();
                        });
                })
                .StartAsync();

            var response = await host.GetTestClient().GetAsync("/images/3");

            Assert.Equal("image/bmp", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task InvokeMiddleware_RequestContainsImages_ReturnsContentTypeImgBmp()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Request.Path = "/images/3";
            var middleware = new ImageCachingMiddleware(next: (innerHttpContext) => Task.CompletedTask);

            await middleware.Invoke(context);

            Assert.Equal("image/bmp", context.Response.ContentType);

        }

        [Fact]
        public async Task InvokeMiddleware_RequestContainsImages_DirectoryToStoreExists()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Request.Path = "/images/test";
            var middleware = new ImageCachingMiddleware(next: (innerHttpContext) => Task.CompletedTask);

            await middleware.Invoke(context);

            Assert.True(Directory.Exists(_pathToImageFolder));
        }


        [Fact]
        public async Task InvokeMiddleware_RequestContainsImages_FileTestnameExistsInCache()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var pathToFile = Path.Combine(_pathToImageFolder, "testname.bmp");
            context.Request.Path = "/images/testname";
            var middleware = new ImageCachingMiddleware(next: (innerHttpContext) => Task.CompletedTask);

            await middleware.Invoke(context);

            Assert.True(File.Exists(pathToFile));
        }

        [Fact]
        public async Task InvokeMiddleware_RequestContainsImages_ReturnsFromLocalCacheFile()
        {

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var pathToFile = Path.Combine(AppContext.BaseDirectory, "images/testname.bmp");
            CreateTestDirectory();
            await File.WriteAllTextAsync(pathToFile, "Text");
            context.Request.Path = "/images/testname";
            var middleware = new ImageCachingMiddleware(next: (innerHttpContext) => Task.CompletedTask);

            await middleware.Invoke(context);

            Assert.True(File.Exists(pathToFile));
            Assert.NotEqual(0, context.Response.Body.Length);
        }

        [Fact]
        public async Task InvokeMiddleware_RequestContainsImages_DoesNotStoreImageIfCacheFull()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var pathToFile = Path.Combine(AppContext.BaseDirectory, "images/testname.bmp");
            var pathToSecondFile = Path.Combine(AppContext.BaseDirectory, "images/testname2.bmp");
            CreateTestDirectory();
            await File.WriteAllTextAsync(Path.Combine(_pathToImageFolder, "firstimage.bmp"), "Text1");
            await File.WriteAllTextAsync(Path.Combine(_pathToImageFolder, "secondimage.bmp"), "Text2");
            context.Request.Path = "/images/testname";
            var middleware = new ImageCachingMiddleware(next: (innerHttpContext) => Task.CompletedTask, new ImageCachingOptions("images", 3, new TimeSpan(1,0,0), 7));

            await middleware.Invoke(context);

            Assert.True(File.Exists(pathToFile));
            Assert.NotEqual(0, context.Response.ContentLength);

            context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            await middleware.Invoke(context);

            Assert.False(File.Exists(pathToSecondFile));
            Assert.Null(context.Response.ContentLength);
        }

        [Fact]
        public async Task InvokeMiddleware_RequestContainsImages_FileIsWrittenAftercleanUp()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var pathToFileStored = Path.Combine(AppContext.BaseDirectory, "images/testname.bmp");
            var pathToFileToStore = Path.Combine(AppContext.BaseDirectory, "images/testnamesecond.bmp");
            CreateTestDirectory();
            await File.WriteAllTextAsync(pathToFileStored, "Text");
            context.Request.Path = "/images/testnamesecond";
            var middleware = new ImageCachingMiddleware(next: (innerHttpContext) => Task.CompletedTask,
                new ImageCachingOptions("images", 3, new TimeSpan(1, 0, 0), 7));
            Directory.SetCreationTime(_pathToImageFolder, DateTime.Now.AddDays(-8));

            await middleware.Invoke(context);

            Assert.True(File.Exists(pathToFileToStore));
            Assert.False(File.Exists(pathToFileStored));
        }

        public void Dispose()
        {
            if (Directory.Exists(_pathToImageFolder)) {
                Directory.Delete(_pathToImageFolder, true);
            }
        }

        private void CreateTestDirectory()
        {
            if (!Directory.Exists(_pathToImageFolder))
            {
                Directory.CreateDirectory(_pathToImageFolder);
            }
        }
    }
}

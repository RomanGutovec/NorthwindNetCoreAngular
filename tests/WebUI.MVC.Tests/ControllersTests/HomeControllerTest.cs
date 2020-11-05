using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebUI.MVC.Controllers;
using WebUI.MVC.Models;
using Xunit;

namespace WebUI.MVC.Tests
{
    public class HomeControllerTest
    {
        [Fact]
        public void HomeIndex_ReturnsAViewResult()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            //Act
            var result = controller.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void HomeError_ReturnsAViewResult_WithErrorModel()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            controller.HttpContext.Features.Set<IExceptionHandlerFeature>(new ExceptionHandlerFeature {
                Error = new UnauthorizedAccessException()
            });

            //Act
            var result = controller.Error();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);
        }
    }
}
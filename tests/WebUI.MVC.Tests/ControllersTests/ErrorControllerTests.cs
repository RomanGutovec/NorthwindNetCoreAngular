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
    public class ErrorControllerTests
    {
        [Fact]
        public void ErrorIndex_ReturnsAViewResult_WithErrorModel()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<ErrorController>>();
            var controller = new ErrorController(loggerMock.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            controller.HttpContext.Features.Set<IExceptionHandlerFeature>(new ExceptionHandlerFeature {
                Error = new UnauthorizedAccessException()
            });

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);
        }
    }
}

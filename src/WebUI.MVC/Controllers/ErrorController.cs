using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using WebUI.MVC.Models;

namespace WebUI.MVC.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var error = this.HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            _logger.LogError("Something went wrong", error);

            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ShowAdditionalInfo = true,
                Message = error.Message,
                Trace = error.StackTrace
            });
        }
    }
}
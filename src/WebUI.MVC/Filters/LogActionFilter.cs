using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebUI.MVC.Filters
{
    public class LogActionFilter : Attribute, IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private readonly bool _shouldLogParameters;
        private string MessageExecuted => "Action {DisplayName} has been executed.";
        public string MessageExecuting =>
            _shouldLogParameters
                ? "Action {DisplayName} with parameter {QueryString} is working."
                : "Action {DisplayName} is working.";

        public LogActionFilter(ILogger<LogActionFilter> logger, bool shouldLogParameters = false)
        {
            _logger = logger;
            _shouldLogParameters = shouldLogParameters;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(MessageExecuting, context.ActionDescriptor.DisplayName, context.HttpContext.Request.QueryString);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation(MessageExecuted, context.ActionDescriptor.DisplayName);
        }
    }
}

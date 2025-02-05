using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Pets.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action {ActionName} executing with arguments: {Arguments}",
                context.ActionDescriptor.DisplayName, context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Action {ActionName} executed.", context.ActionDescriptor.DisplayName);
        }
    }
}

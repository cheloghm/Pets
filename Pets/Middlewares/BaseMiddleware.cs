using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Pets.Middlewares
{
    /// <summary>
    /// An abstract base middleware that catches exceptions and delegates error handling.
    /// </summary>
    public abstract class BaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        protected BaseMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception.
                _logger.LogError(ex, "An error occurred.");
                // Delegate error handling to the concrete middleware.
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception and creates an appropriate HTTP response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="ex">The exception that occurred.</param>
        protected abstract Task HandleExceptionAsync(HttpContext context, Exception ex);
    }
}

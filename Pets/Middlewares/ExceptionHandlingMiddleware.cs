using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pets.Middlewares
{
    /// <summary>
    /// Middleware to handle exceptions and return a standardized JSON response.
    /// </summary>
    public class ExceptionHandlingMiddleware : BaseMiddleware
    {
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
            : base(next, logger)
        {
        }

        protected override async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Set the HTTP status code and content type.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // Create a response object with a friendly error message.
            var response = new
            {
                message = "An unexpected error occurred. Please try again later."
            };

            // Serialize the response object to JSON.
            var jsonResponse = JsonSerializer.Serialize(response);

            // Write the JSON response to the HTTP response.
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Pets.Helpers
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Creates a standardized error response object.
        /// </summary>
        public static ObjectResult CreateErrorResponse(IEnumerable<string> errors, int statusCode = 400)
        {
            var errorResponse = new
            {
                Errors = errors
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };
        }
    }
}

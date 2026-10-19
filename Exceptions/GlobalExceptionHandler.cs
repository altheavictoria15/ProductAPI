using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title, detail) = exception switch
            {
                NotFoundException =>
                    (StatusCodes.Status404NotFound,
                     "Product not found",
                     exception.Message),

                ConflictException =>
                    (StatusCodes.Status409Conflict,
                     "Product conflict",
                     exception.Message),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                     "An unexpected error occurred",
                     "Something went wrong while processing your request. Please try again later.")
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "An unexpected error occurred while processing a request.");
            }
            else
            {
                _logger.LogInformation(
                    "Request failed with status {StatusCode}: {Message}",
                    statusCode,
                    exception.Message);
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            problem.Extensions["traceId"] =
                httpContext.TraceIdentifier;

            await httpContext.Response.WriteAsJsonAsync(
                problem,
                cancellationToken);

            return true;
        }
    }
}
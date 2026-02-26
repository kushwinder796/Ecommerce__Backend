using Microsoft.AspNetCore.Diagnostics;

namespace Ecommerce_13.Comman
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                KeyNotFoundException => 404,
                ArgumentException => 400,
                UnauthorizedAccessException => 401,
                _ => 500
            };

            var message = exception switch
            {
                KeyNotFoundException => exception.Message,
                ArgumentException => exception.Message,
                UnauthorizedAccessException => "Unauthorized",
                _ => "Internal server error"
            };

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(
                ApiResponse<string>.FailResult(message, statusCode),
                cancellationToken
            );

            return true;
        }
    }
}

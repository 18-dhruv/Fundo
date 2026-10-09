using BusinessLayer.Exceptions;
using ModelLayer.Models.Common;

namespace FundoNotes.Middleware
{
   
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FundooException ex)
            {
                // Expected errors: not found, conflict, bad login...
                await WriteErrorAsync(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                // Bugs and infrastructure failures: log the details, hide them from the client.
                _logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
                await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Something went wrong. Please try again later.");
            }
        }

        private static Task WriteErrorAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(ResponseModel<object>.Failure(message));
        }
    }
}

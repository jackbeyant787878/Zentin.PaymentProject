using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Zentin.Payment.Infrastructure.Middleware
{
    public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                // 响应已开始写出就无法再改写为 ProblemDetails，交回服务器处理
                if (context.Response.HasStarted)
                {
                    throw;
                }

                var (statusCode, title, detail) = ex switch
                {
                    FluentValidation.ValidationException vex =>
                        (StatusCodes.Status400BadRequest, "Validation Failed", string.Join("; ", vex.Errors.Select(e => e.ErrorMessage))),
                    ArgumentException or InvalidOperationException =>
                        (StatusCodes.Status400BadRequest, "Bad Request", ex.Message),
                    KeyNotFoundException =>
                        (StatusCodes.Status404NotFound, "Not Found", ex.Message),
                    OperationCanceledException =>
                        (StatusCodes.Status499ClientClosedRequest, "Client Closed Request", "Request was cancelled."),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
                };

                context.Response.Clear();
                context.Response.StatusCode = statusCode;

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = detail,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
            }
        }
    }
}

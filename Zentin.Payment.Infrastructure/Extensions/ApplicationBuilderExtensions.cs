using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;
using Zentin.Payment.Infrastructure.Middleware;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        // 异常处理须最先注册（最外层），才能兜住内层中间件与端点抛出的异常
        public static IApplicationBuilder UseZentinExceptionHandling(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseZentinRequestLogging(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static IApplicationBuilder UseZentinCors(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);
            return app.UseCors(CorsServiceCollectionExtensions.PolicyName);
        }

        // 端点映射类扩展作用于 IEndpointRouteBuilder（WebApplication 实现它）
        public static IEndpointRouteBuilder UseZentinSwagger(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);
            endpoints.MapOpenApi();
            endpoints.MapScalarApiReference();
            return endpoints;
        }
    }
}

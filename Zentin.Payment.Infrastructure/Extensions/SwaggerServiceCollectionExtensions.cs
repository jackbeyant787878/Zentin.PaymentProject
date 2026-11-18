using Microsoft.Extensions.DependencyInjection;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class SwaggerServiceCollectionExtensions
    {
        // .NET 10 原生 OpenApi 文档 + Scalar UI，四宿主统一；title 由各宿主传入
        public static IServiceCollection AddZentinSwagger(this IServiceCollection services, string title)
        {
            services.AddEndpointsApiExplorer();
            services.AddOpenApi(options =>
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info.Title = title;
                    document.Info.Version = "v1";
                    return Task.CompletedTask;
                }));
            return services;
        }
    }
}
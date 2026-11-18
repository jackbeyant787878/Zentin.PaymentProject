using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class CorsServiceCollectionExtensions
    {
        public const string PolicyName = "CorsPolicy";

        // 配置了 Cors:AllowedOrigins 白名单则严格校验；未配置则放行所有来源（本地开发）
        public static IServiceCollection AddZentinCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors:AllowedOrigins").GetChildren()
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToArray();

            services.AddCors(o => o.AddPolicy(PolicyName, p =>
            {
                p.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                if (origins.Length > 0)
                {
                    p.WithOrigins(origins!);
                }
                else
                {
                    p.SetIsOriginAllowed(_ => true);
                }
            }));
            return services;
        }
    }
}
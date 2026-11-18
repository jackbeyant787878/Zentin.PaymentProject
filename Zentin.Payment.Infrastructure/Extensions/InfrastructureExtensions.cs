using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zentin.Payment.Infrastructure.Data;
using Zentin.Payment.Infrastructure.HealthChecks;
using Zentin.Payment.Infrastructure.Interceptors;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddZentinInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 对象池复用 DbContext 实例，降低高并发下的分配开销；要求单一 DbContextOptions 构造（已满足）
            // 池化禁止在 OnConfiguring 改 options，故拦截器与详细错误在此注入（拦截器无状态，可共享）
            services.AddDbContextPool<PaymentDbContext>(opts =>
                opts.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null))
                    .AddInterceptors(new AuditFieldInterceptor())
                    .EnableDetailedErrors());

            services.AddZentinRepositories();
            services.AddZentinRedis(configuration);
            services.AddZentinRabbitMQ(configuration);

            // 未配置 ConsulConfig:RegistryAddress 时该扩展自动 no-op，本地开发可不接注册中心
            services.AddConsulRegistry(configuration);

            services.AddHealthChecks()
                .AddDbContextCheck<PaymentDbContext>("sqlserver", tags: ["db"])
                .AddCheck<RedisHealthCheck>("redis", tags: ["cache"])
                .AddCheck<RabbitMQHealthCheck>("rabbitmq", tags: ["mq"]);

            // 出站 HTTP 统一韧性（重试/熔断/超时/限流），供后续渠道适配器等客户端继承
            services.AddZentinResilience(configuration);

            return services;
        }
    }
}

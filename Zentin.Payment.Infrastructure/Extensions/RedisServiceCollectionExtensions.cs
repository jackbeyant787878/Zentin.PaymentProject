using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Zenith.Extensions.Redis;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddZentinRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("缺少配置 ConnectionStrings:Redis。");

            var database = int.TryParse(configuration["Redis:Database"], out var db) ? db : 0;

            // AbortOnConnectFail=false：Redis 短暂不可用时后台重连，而不是在首次连接抛异常
            //（否则健康检查在 DI 实例化阶段就会拿到 RedisConnectionException，穿透到异常中间件）
            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(options);
            });
            // CSRedisCore 在全局命名空间也声明了抽象类 RedisHelper，裸名会绑定到它，必须全限定
            services.AddScoped<IRedisHelper>(sp =>
                new Zenith.Extensions.Redis.RedisHelper(sp.GetRequiredService<IConnectionMultiplexer>(), database));

            return services;
        }
    }
}

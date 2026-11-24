using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class CapServiceCollectionExtensions
    {
        /// <summary>
        /// CAP 本地消息表（outbox）：SqlServer 存储与业务同库同事务，RabbitMQ 传输，至少一次投递。
        /// Cap.Published / Cap.Received 表由 CAP 启动时自建自管，不走 EF 迁移。
        /// </summary>
        public static IServiceCollection AddZentinCap(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("缺少配置 ConnectionStrings:DefaultConnection。");

            var hostName = configuration["RabbitMQ:HostName"]
                ?? throw new InvalidOperationException("缺少配置 RabbitMQ:HostName。");

            services.AddCap(options =>
            {
                options.UseSqlServer(connectionString);

                options.UseRabbitMQ(mq =>
                {
                    mq.HostName = hostName;
                    mq.Port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
                    mq.UserName = configuration["RabbitMQ:UserName"] ?? "guest";
                    mq.Password = configuration["RabbitMQ:Password"] ?? "guest";
                    mq.VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
                    mq.ExchangeName = configuration["Cap:ExchangeName"] ?? "zentin.cap";
                });

                // 每服务独立消费组，四宿主共用一份配置时按服务名区分
                options.DefaultGroupName = configuration["Cap:GroupName"] ?? "zentin.payment";
                options.FailedRetryCount = int.TryParse(configuration["Cap:FailedRetryCount"], out var r) ? r : 5;

                options.UseDashboard();
            });

            return services;
        }
    }
}
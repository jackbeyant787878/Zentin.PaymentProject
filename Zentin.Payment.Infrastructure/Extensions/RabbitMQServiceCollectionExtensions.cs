using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Zenith.Extensions.RabbitMQ;
using Zenith.RabbitMQ.Core;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static IServiceCollection AddZentinRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var hostName = configuration["RabbitMQ:HostName"]
                ?? throw new InvalidOperationException("缺少配置 RabbitMQ:HostName。");

            var port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
            var userName = configuration["RabbitMQ:UserName"] ?? "guest";
            var password = configuration["RabbitMQ:Password"] ?? "guest";
            var virtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";

            services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost
            });
            services.AddSingleton<RabbitMQPersistentConnection>();
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

            return services;
        }
    }
}

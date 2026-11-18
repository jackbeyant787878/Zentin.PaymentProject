using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Zenith.Extensions.RabbitMQ;
using Zenith.Extensions.Redis;
using Zentin.Payment.Infrastructure.Extensions;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class BackingServiceRegistrationTests
    {
        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Redis"] = "localhost:6379",
                    ["Redis:Database"] = "0",
                    ["RabbitMQ:HostName"] = "localhost",
                    ["RabbitMQ:Port"] = "5672",
                    ["RabbitMQ:UserName"] = "guest",
                    ["RabbitMQ:Password"] = "guest",
                    ["RabbitMQ:VirtualHost"] = "/"
                })
                .Build();

        [TestMethod]
        public void Redis_services_are_registered()
        {
            var services = new ServiceCollection();
            services.AddZentinRedis(BuildConfiguration());

            var multiplexer = services.Single(d => d.ServiceType == typeof(IConnectionMultiplexer));
            var helper = services.Single(d => d.ServiceType == typeof(IRedisHelper));

            Assert.AreEqual(ServiceLifetime.Singleton, multiplexer.Lifetime);
            Assert.AreEqual(ServiceLifetime.Scoped, helper.Lifetime);
        }

        [TestMethod]
        public async Task RabbitMQ_producer_resolves_without_a_broker()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinRabbitMQ(BuildConfiguration());

            // RabbitMQPersistentConnection 只实现 IAsyncDisposable，同步 Dispose 容器会抛异常
            await using var provider = services.BuildServiceProvider();

            Assert.IsNotNull(provider.GetRequiredService<IRabbitMQProducer>());
        }

        [TestMethod]
        public void RabbitMQ_without_host_name_fails_fast()
        {
            var services = new ServiceCollection();
            var empty = new ConfigurationBuilder().Build();

            Assert.ThrowsExactly<InvalidOperationException>(() => services.AddZentinRabbitMQ(empty));
        }

        [TestMethod]
        public void Redis_without_connection_string_fails_fast()
        {
            var services = new ServiceCollection();
            var empty = new ConfigurationBuilder().Build();

            Assert.ThrowsExactly<InvalidOperationException>(() => services.AddZentinRedis(empty));
        }
    }
}

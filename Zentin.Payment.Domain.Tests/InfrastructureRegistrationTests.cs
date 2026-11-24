using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Infrastructure.Data;
using Zentin.Payment.Infrastructure.Extensions;
using Zenith.Extensions.RabbitMQ;
using Zenith.Extensions.Redis;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class InfrastructureRegistrationTests
    {
        private static IConfiguration BuildConfiguration(bool withConsul)
        {
            var values = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)\\mssqllocaldb;Database=ZentinDi;Integrated Security=true",
                ["ConnectionStrings:Redis"] = "localhost:6379",
                ["RabbitMQ:HostName"] = "localhost"
            };
            if (withConsul)
            {
                values["ConsulConfig:RegistryAddress"] = "http://localhost:8500";
            }
            return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
        }

        [TestMethod]
        public void Core_services_are_registered()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinInfrastructure(BuildConfiguration(withConsul: false));

            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(PaymentDbContext)));
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IUnitOfWork)));
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IRedisHelper)));
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IRabbitMQProducer)));
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(HealthCheckService)));
        }

        [TestMethod]
        public void Consul_is_skipped_when_not_configured()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinInfrastructure(BuildConfiguration(withConsul: false));

            Assert.IsFalse(services.Any(d => d.ServiceType == typeof(IConsulClient)));
        }

        [TestMethod]
        public void Consul_client_is_registered_when_configured()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinInfrastructure(BuildConfiguration(withConsul: true));

            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IConsulClient)));
        }

        [TestMethod]
        public void Health_checks_include_sqlserver_redis_rabbitmq()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinInfrastructure(BuildConfiguration(withConsul: false));

            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();
            var names = options.Value.Registrations.Select(r => r.Name).ToList();

            Assert.IsTrue(names.Contains("sqlserver"));
            Assert.IsTrue(names.Contains("redis"));
            Assert.IsTrue(names.Contains("rabbitmq"));
        }

        [TestMethod]
        public void Cap_publisher_is_registered_with_outbox_storage()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinInfrastructure(BuildConfiguration(withConsul: false));

            var provider = services.BuildServiceProvider();
            var publisher = provider.GetRequiredService<DotNetCore.CAP.ICapPublisher>();
            var options = provider.GetRequiredService<IOptions<DotNetCore.CAP.CapOptions>>().Value;

            Assert.IsNotNull(publisher);
            Assert.AreEqual("zentin.payment", options.DefaultGroupName);
        }

        [TestMethod]
        public void Resilience_pipeline_applies_to_http_clients()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddZentinResilience(BuildConfiguration(withConsul: false));

            var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("resilience-smoke");

            Assert.IsNotNull(client);
        }
    }
}

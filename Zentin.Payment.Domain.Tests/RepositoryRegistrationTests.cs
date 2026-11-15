using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.IRepositories;
using Zentin.Payment.Infrastructure.Data;
using Zentin.Payment.Infrastructure.Extensions;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class RepositoryRegistrationTests
    {
        private static ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddDbContext<PaymentDbContext>(o =>
                o.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ZentinDiValidation;Integrated Security=true"));
            services.AddZentinRepositories();
            return services.BuildServiceProvider();
        }

        private static Type[] RepositoryPorts() =>
            typeof(IPayOrderRepository).Assembly.GetTypes()
                .Where(t => t.IsInterface && t.Namespace == "Zentin.Payment.Domain.IRepositories")
                .OrderBy(t => t.Name)
                .ToArray();

        [TestMethod]
        public void Every_repository_port_resolves_from_the_container()
        {
            using var provider = BuildProvider();
            using var scope = provider.CreateScope();

            var failures = new List<string>();
            foreach (var port in RepositoryPorts())
            {
                try
                {
                    scope.ServiceProvider.GetRequiredService(port);
                }
                catch (Exception ex)
                {
                    failures.Add($"{port.Name}: {ex.GetType().Name} {ex.Message}");
                }
            }

            CollectionAssert.AreEqual(Array.Empty<string>(), failures);
        }

        [TestMethod]
        public void Repository_port_count_matches_the_expected_baseline()
        {
            Assert.HasCount(45, RepositoryPorts());
        }

        [TestMethod]
        public void Unit_of_work_is_registered()
        {
            var services = new ServiceCollection();
            services.AddZentinRepositories();

            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IUnitOfWork)));
        }
    }
}
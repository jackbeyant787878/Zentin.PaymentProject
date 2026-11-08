using Microsoft.EntityFrameworkCore;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.Tests
{
    [TestClass]
    public sealed class PaymentDbContextModelTests
    {
        private static PaymentDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ZentinModelValidation;Integrated Security=true")
                .Options;

            return new PaymentDbContext(options);
        }

        [TestMethod]
        public void Model_builds_without_validation_errors()
        {
            using var context = CreateContext();

            Assert.IsNotNull(context.Model);
        }

        [TestMethod]
        public void Each_table_is_mapped_by_exactly_one_entity_type()
        {
            using var context = CreateContext();

            var collisions = context.Model.GetEntityTypes()
                .GroupBy(e => e.GetTableName())
                .Where(g => g.Count() > 1)
                .Select(g => $"{g.Key} <- {string.Join(" | ", g.Select(e => e.ClrType.FullName))}")
                .OrderBy(s => s)
                .ToArray();

            CollectionAssert.AreEqual(Array.Empty<string>(), collisions);
        }
    }
}

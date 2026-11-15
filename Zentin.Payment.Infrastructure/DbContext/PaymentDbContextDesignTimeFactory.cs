using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Zentin.Payment.Infrastructure.Data
{
    public sealed class PaymentDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ZENTIN_DESIGN_CONNECTION")
                ?? "Server=(localdb)\\mssqllocaldb;Database=ZentinPayment;Integrated Security=true";

            var options = new DbContextOptionsBuilder<PaymentDbContext>()
                .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName))
                .Options;

            return new PaymentDbContext(options);
        }
    }
}
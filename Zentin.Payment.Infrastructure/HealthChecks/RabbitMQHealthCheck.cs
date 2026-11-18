using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zenith.RabbitMQ.Core;

namespace Zentin.Payment.Infrastructure.HealthChecks
{
    public sealed class RabbitMQHealthCheck(RabbitMQPersistentConnection connection) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var ok = connection.IsConnected || await connection.TryConnectAsync(cancellationToken);
                return ok
                    ? HealthCheckResult.Healthy("rabbitmq connected")
                    : HealthCheckResult.Unhealthy("rabbitmq unreachable");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("rabbitmq unreachable", ex);
            }
        }
    }
}
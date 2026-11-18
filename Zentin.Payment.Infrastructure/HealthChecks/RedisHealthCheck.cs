using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Zentin.Payment.Infrastructure.HealthChecks
{
    public sealed class RedisHealthCheck(IConnectionMultiplexer multiplexer) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var latency = await multiplexer.GetDatabase().PingAsync();
                return HealthCheckResult.Healthy($"ping {latency.TotalMilliseconds:F0}ms");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("redis unreachable", ex);
            }
        }
    }
}
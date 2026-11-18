using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zentin.Payment.Infrastructure.Extensions
{
    public static class ResilienceServiceCollectionExtensions
    {
        // AddStandardResilienceHandler 是微软官方的 Polly v8 DI 封装，内部由以下 Polly 策略组合：
        //   RetryStrategy(CircuitBreakerStrategy(TimeoutStrategy)) + RateLimiter + TotalRequestTimeout
        // 以下把各策略关键参数显式展开为配置项，参数类型即 Polly 策略选项（Retry/CircuitBreaker/Timeout）
        public static IServiceCollection AddZentinResilience(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddResilienceEnricher();

            var maxRetryAttempts = GetInt(configuration, "Resilience:MaxRetryAttempts", 3);
            var totalTimeoutSeconds = GetInt(configuration, "Resilience:TotalRequestTimeoutSeconds", 30);
            var failureRatio = GetDouble(configuration, "Resilience:CircuitBreaker:FailureRatio", 0.1);
            var minimumThroughput = GetInt(configuration, "Resilience:CircuitBreaker:MinimumThroughput", 100);
            var samplingDurationSeconds = GetInt(configuration, "Resilience:CircuitBreaker:SamplingDurationSeconds", 30);
            var breakDurationSeconds = GetInt(configuration, "Resilience:CircuitBreaker:BreakDurationSeconds", 5);

            services.ConfigureHttpClientDefaults(builder => builder.AddStandardResilienceHandler(options =>
            {
                // Polly.Retry.RetryStrategyOptions
                options.Retry.MaxRetryAttempts = maxRetryAttempts;

                // Polly.CircuitBreaker.CircuitBreakerStrategyOptions
                options.CircuitBreaker.FailureRatio = failureRatio;
                options.CircuitBreaker.MinimumThroughput = minimumThroughput;
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(samplingDurationSeconds);
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(breakDurationSeconds);

                // Polly.Timeout.TimeoutStrategyOptions（整条链路总超时）
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(totalTimeoutSeconds);
            }));

            return services;
        }

        private static int GetInt(IConfiguration configuration, string key, int fallback)
            => int.TryParse(configuration[key], out var value) ? value : fallback;

        private static double GetDouble(IConfiguration configuration, string key, double fallback)
            => double.TryParse(configuration[key], NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;
    }
}
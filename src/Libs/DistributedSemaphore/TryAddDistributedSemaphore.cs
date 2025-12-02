using DistributedSemaphore.Semaphore;
using DistributedSemaphore.SemaphoreFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DistributedSemaphore
{
    public static class TryAddDistributedSemaphore
    {

        public static IServiceCollection AddDistributedSemaphore(
        this IServiceCollection services,
        string semaphoreName,
        int permits,
        TimeSpan leaseTime)
        {
            if (string.IsNullOrWhiteSpace(semaphoreName))
                throw new ArgumentException("У семафора должно быть название.", nameof(semaphoreName));
            if (permits < 0)
                throw new ArgumentOutOfRangeException(nameof(permits));
            if (leaseTime <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(leaseTime));

            // Регистрируем конфиг в DI как singleton (для watchdog’ов и инициализации)
            services.Configure<List<SemaphoreConfig>>(options =>
            {
                options.Add(new SemaphoreConfig
                {
                    Name = semaphoreName,
                    Permits = permits,
                    LeaseTime = leaseTime
                });
            });

            return services;
        }

        public static IServiceCollection AddDistributedSemaphoreInfrastructure(this IServiceCollection services)
        {
            // Зарегистрируем фабрику
            services.TryAddSingleton<IDistributedSemaphoreFactory>(sp =>
            {
                var configs = sp.GetRequiredService<IOptions<List<SemaphoreConfig>>>().Value;
                var leaseTimes = configs.ToDictionary(c => c.Name, c => c.LeaseTime);
                var redis = sp.GetRequiredService<ConnectionMultiplexer>();
                return new DistributedSemaphoreFactory(redis);
            });

            services.AddHostedService<DistributedSemaphoreHostedService>();

            return services;
        }
    }

    internal class SemaphoreConfig
    {
        public string Name { get; set; } = null!;
        public int Permits { get; set; }
        public TimeSpan LeaseTime { get; set; }
    }

    internal class DistributedSemaphoreHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DistributedSemaphoreHostedService> _logger;

        public DistributedSemaphoreHostedService(
            IServiceProvider serviceProvider,
            ILogger<DistributedSemaphoreHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            var configs = scope.ServiceProvider.GetRequiredService<IOptions<List<SemaphoreConfig>>>().Value;
            var redis = scope.ServiceProvider.GetRequiredService<ConnectionMultiplexer>();

            foreach (var config in configs)
            {
                try
                {
                    await RedisSemaphoreInitializer.InitSemaphoreAsync(redis, config.Name, config.Permits);
                    _logger.LogInformation("Initialized semaphore '{Name}' with {Permits} permits", config.Name, config.Permits);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to initialize semaphore '{Name}'", config.Name);
                    throw;
                }
            }

            var watchdogService = scope.ServiceProvider.GetRequiredService<IHostedService>();
            var serviceScopeFactory = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

            foreach (var config in configs)
            {
                var watchdog = new SemaphoreWatchdog(
                    redis,
                    loggerFactory.CreateLogger<SemaphoreWatchdog>(),
                    config.Name);

                // Запустим watchdog в фоне
                _ = Task.Run(() => watchdog.StartAsync(ct), ct);
            }
        }

        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    }
}

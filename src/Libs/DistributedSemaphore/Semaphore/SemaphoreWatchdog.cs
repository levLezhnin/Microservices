using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DistributedSemaphore.Semaphore
{
    internal class SemaphoreWatchdog : BackgroundService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly ILogger<SemaphoreWatchdog> _logger;
        private readonly string _semaphoreName;

        public SemaphoreWatchdog(ConnectionMultiplexer redis, ILogger<SemaphoreWatchdog> logger, string semaphoreName)
        {
            _redis = redis;
            _logger = logger;
            _semaphoreName = semaphoreName;
        }

        protected async override Task ExecuteAsync(CancellationToken ct)
        {
            var keys = new SemaphoreKeys(_semaphoreName);
            var db = _redis.GetDatabase();

            const string recoverScript = @"
                if redis.call('EXISTS', KEYS[2]) == 1 then
                    return 0  -- ещё жив
                end
                local current = tonumber(redis.call('GET', KEYS[1]) or '0')
                local max = tonumber(ARGV[1])
                if current < max then
                    redis.call('INCR', KEYS[1])
                    return 1
                else
                    return -1
                end
            ";

            while (!ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    // Получаем все holder-ключи
                    var holderKeys = await keys.GetAllHolderKeysAsync(db, ct);

                    var configKey = keys.ConfigKey;
                    var maxPermits = await db.StringGetAsync(configKey);
                    if (!long.TryParse(maxPermits, out var max)) continue;

                    foreach (var holderKey in holderKeys)
                    {
                        var result = (long)await db.ScriptEvaluateAsync(
                            recoverScript,
                            keys: new RedisKey[] { keys.CounterKey, holderKey },
                            values: new RedisValue[] { max });

                        if (result == 1)
                            _logger.LogWarning("Watchdog recovered permit for expired holder: {Holder}", holderKey);
                        else if (result == -1)
                            _logger.LogInformation("Watchdog: redundant recovery attempt for {Holder}", holderKey);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Watchdog error");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), ct);
            }
        }
    }
}

using Consul;
using StackExchange.Redis;

namespace DistributedSemaphore.Semaphore
{
    public class RedisDistributedSemaphore : IDistributedSemaphore, IDisposable
    {
        private static readonly TimeSpan DEFAULT_LOCK_EXPIRATION_TIME = TimeSpan.FromMinutes(10);

        private IDatabase _redis;
        private SemaphoreKeys _keys;
        
        private readonly string _holderKey;
        private bool _isHeld;

        private TimeSpan _expirationTime = DEFAULT_LOCK_EXPIRATION_TIME;
        private CancellationTokenSource? _cancellationTokenSource;

        public RedisDistributedSemaphore(
            ConnectionMultiplexer connectionMultiplexer,
            string name
        ) : this(connectionMultiplexer, name, DEFAULT_LOCK_EXPIRATION_TIME)
        {

        }

        public RedisDistributedSemaphore(
            ConnectionMultiplexer connectionMultiplexer,
            string name,
            TimeSpan expirationTime
        )
        {
            _redis = connectionMultiplexer.GetDatabase();
            _keys = new SemaphoreKeys(name);
            _holderKey = Guid.NewGuid().ToString();
            _expirationTime = expirationTime;
            _isHeld = false;
        }

        public bool IsHeld => _isHeld;

        private async Task EnsureInitializedAsync()
        {
            var value = await _redis.StringGetAsync(_keys.CounterKey);
            if (!value.HasValue)
            {
                throw new InvalidOperationException(
                    $"Семафор не инициализован. " +
                    "Вызовите RedisSemaphoreInitializer.InitializeAsync(redis, \"{name}\", permits).");
            }

            if (!long.TryParse(value, out var permits) || permits <= 0)
            {
                throw new InvalidOperationException(
                    $"Семафор содержит неправильное значение: '{value}'. Ожидалось положительное целое число.");
            }
        }

        public async Task<CancellationToken> Acquire(CancellationToken ct = default)
        {
            if (IsHeld)
            {
                throw new InvalidOperationException("Блокировка уже захвачена.");
            }

            await EnsureInitializedAsync();

            var luaAcquire = @"
                local permits = redis.call('GET', KEYS[3])
                if not permits then
                    return redis.error_reply('not_initialized')
                end
                local free = redis.call('GET', KEYS[1])
                if not free or tonumber(free) <= 0 then
                    return 0
                end
                redis.call('DECR', KEYS[1])
                redis.call('SET', KEYS[2], '1', 'PX', ARGV[1])
                return 1
            ";

            var success = (bool) await _redis.ScriptEvaluateAsync(
                        luaAcquire,
                        keys: new RedisKey[] { _keys.CounterKey, _holderKey, _keys.ConfigKey },
                        values: new RedisValue[] { _expirationTime.TotalMilliseconds }
            );

            if (!success)
            {
                throw new OperationCanceledException($"Не получилось взять блокировку у семафора, т.к. нет доступных блокировок.", ct);
            }

            _isHeld = true;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
            return _cancellationTokenSource.Token;
        }

        public async Task Release(CancellationToken ct = default)
        {
            if (!IsHeld)
            {
                throw new InvalidOperationException("Захваченной блокировки нет, освобождать нечего.");
            }

            var luaRelease = @"
                local holderExists = redis.call('EXISTS', KEYS[2])
                if holderExists == 0 then
                    return redis.error_reply('not_held')
                end
                redis.call('DEL', KEYS[2])
                redis.call('INCR', KEYS[1])
            ";

            try
            {
                await _redis.ScriptEvaluateAsync(
                    luaRelease,
                    keys: new RedisKey[] { _keys.CounterKey, _holderKey },
                    values: new RedisValue[] { _holderKey });
            }
            catch (RedisServerException ex) when (ex.Message.Contains("not_held"))
            {
                throw new InvalidOperationException(
                    $"Не получилось освободить блокировку: захваченная блокировка либо истекла, либо была неправильно захвачена.", ex);
            }

            _isHeld = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public async Task Destroy(CancellationToken ct = default)
        {
            if (_isHeld)
            {
                await Release(ct);
            }

            const string script = @"
                redis.call('DEL', KEYS[1])  -- counter
                redis.call('DEL', KEYS[2])  -- config
                local keys = redis.call('KEYS', ARGV[1])
                if #keys > 0 then
                    redis.call('DEL', unpack(keys))
                end
                return #keys
            ";

            var deleted = (long)await _redis.ScriptEvaluateAsync(
                script,
                keys: new RedisKey[] { _keys.CounterKey, _keys.ConfigKey },
                values: new RedisValue[] { _keys.HolderKeyTemplate + "*" });
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}

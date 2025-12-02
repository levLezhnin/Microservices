using StackExchange.Redis;

namespace DistributedSemaphore.Semaphore
{
    public static class RedisSemaphoreInitializer
    {
        public static async Task InitSemaphoreAsync(
            ConnectionMultiplexer multiplexer,
            string semaphoreName,
            int limit
        )
        {
            if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit));

            var db = multiplexer.GetDatabase();
            var keys = new SemaphoreKeys(semaphoreName);

            // Атомарная инициализация с проверкой
            const string script = @"
                local current = redis.call('GET', KEYS[2])
                if current then
                    if tonumber(current) ~= tonumber(ARGV[1]) then
                        return redis.error_reply('mismatch:' .. current)
                    end
                    return 'exists'
                else
                    redis.call('SET', KEYS[1], ARGV[1])
                    redis.call('SET', KEYS[2], ARGV[1])
                    return 'created'
                end
            ";

            try
            {
                await db.ScriptEvaluateAsync(
                    script,
                    keys: new RedisKey[] { keys.CounterKey, keys.ConfigKey },
                    values: new RedisValue[] { limit });
            }
            catch (RedisServerException ex) when (ex.Message.StartsWith("mismatch:"))
            {
                var got = ex.Message["mismatch:".Length..];
                throw new InvalidOperationException(
                    $"Семафор '{semaphoreName}' инициализован с лимит = {got}, запрошено создание с лимитом {limit}");
            }
        }
    }
}

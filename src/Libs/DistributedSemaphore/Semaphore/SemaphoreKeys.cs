using StackExchange.Redis;

namespace DistributedSemaphore.Semaphore
{
    public sealed class SemaphoreKeys
    {
        public string CounterKey { get; }
        public string HolderKeyTemplate { get; }
        public string ConfigKey { get; }

        public SemaphoreKeys(string name)
        {
            CounterKey = $"semaphore:{name}";
            HolderKeyTemplate = $"{CounterKey}:holder:";
            ConfigKey = $"{CounterKey}:config:permits";
        }

        public string GetHolderKey(string holderId) =>
            HolderKeyTemplate + holderId;

        // Для watchdog: получаем все holder-ключи через SCAN (без KEYS)
        public async Task<List<string>> GetAllHolderKeysAsync(IDatabase db, CancellationToken ct = default)
        {
            var server = db.Multiplexer.GetServer(db.Multiplexer.GetEndPoints()[0]);
            var keys = new List<string>();
            var scan = server.Keys(pattern: HolderKeyTemplate + "*", pageSize: 100);

            foreach (var key in scan)
            {
                ct.ThrowIfCancellationRequested();
                keys.Add(key);
            }
            return keys;
        }
    }
}

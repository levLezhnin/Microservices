using Consul;
using StackExchange.Redis;

namespace DistributedSemaphore.SemaphoreFactory
{
    public class DistributedSemaphoreFactory : IDistributedSemaphoreFactory
    {
        private readonly ConnectionMultiplexer _redis;

        public DistributedSemaphoreFactory(
            ConnectionMultiplexer redis
        )
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(_redis));
        }


        public IDistributedSemaphore Create(string semaphoreName)
        {
            return new RedisDistributedSemaphore(_redis, semaphoreName);
        }
    }
}

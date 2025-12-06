using Consul;

namespace DistributedSemaphore.SemaphoreFactory
{
    public interface IDistributedSemaphoreFactory
    {
        IDistributedSemaphore Create(string semaphoreName);
    }
}

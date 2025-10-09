using NotificationApi.Domain.Entities;

namespace NotificationApi.Domain.Interfaces
{
    public interface ISender
    {
        Task sendNotificationAsync(Notification notification);
    }
}

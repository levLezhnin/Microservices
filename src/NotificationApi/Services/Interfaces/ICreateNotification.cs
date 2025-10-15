using NotificationApi.Domain.Entities;

namespace NotificationApi.Services.Interfaces
{
    public interface ICreateNotification
    {
        Task<Notification> createNotification(Guid? srcUserId, Guid destUserId, string notificationType, string message);
    }
}

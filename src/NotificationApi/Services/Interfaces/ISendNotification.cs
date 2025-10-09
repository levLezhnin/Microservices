namespace NotificationApi.Services.Interfaces
{
    public interface ISendNotification
    {
        Task sendNotificationAsync(Guid? srcUserId, Guid destUserId, string notificationType, string message);
    }
}

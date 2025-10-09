using NotificationApi.Domain.Entities;
using NotificationApi.Domain.Interfaces;
using NotificationApi.Services.Interfaces;

namespace NotificationApi.Services.Implementations
{
    public class SendNotification : ISendNotification
    {
        private readonly ICreateNotification _createNotification;
        private readonly ISender _sender;

        public SendNotification(ISender sender, ICreateNotification createNotification)
        {
            _sender = sender;
            _createNotification = createNotification;
        }

        public async Task sendNotificationAsync(Guid? srcUserId, Guid destUserId, string notificationType, string message)
        {
            Notification notification = await _createNotification.createNotification(srcUserId, destUserId, notificationType, message);
            await _sender.sendNotificationAsync(notification);
        }
    }
}

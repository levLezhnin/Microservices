using CoreLib.Common;
using CoreLib.Dto;
using NotificationApi.Domain.Entities;
using NotificationApi.Domain.Interfaces;
using NotificationApi.Services.Interfaces;

namespace NotificationApi.Services.Implementations
{
    public class CreateNotification : ICreateNotification
    {
        private IGetUserInfo _getUserInfo;

        public CreateNotification(IGetUserInfo getUserInfo)
        {
            _getUserInfo = getUserInfo;
        }

        public async Task<Notification> createNotification(Guid? srcUserId, Guid destUserId, string notificationType, string message)
        {
            NotificationType type = (NotificationType) Enum.Parse(typeof(NotificationType), notificationType);

            UserInfoDto srcUserInfo = null;
            UserInfoDto destUserInfo = await _getUserInfo.getUserInfo(destUserId);

            string notificationMessage;
            switch (type)
            {
                case NotificationType.USER_MESSAGE:
                    {
                        if (!srcUserId.HasValue)
                        {
                            throw new ArgumentException("Не могу отправить сообщение от пользователя с id: " + null);
                        }

                        srcUserInfo = await _getUserInfo.getUserInfo(srcUserId.Value);

                        notificationMessage = string.Format("Сообщение от пользователя {0} {1}:\n{2}", srcUserInfo.firstName, srcUserInfo.lastName, message);
                        break;
                    }

                case NotificationType.SYSTEM_MESSAGE:
                    notificationMessage = string.Format("Системное сообщение:\n{0}", message);
                    break;

                default:
                    throw new NotImplementedException("Такой тип уведомления не предусмотрен!");
            }

            return new Notification
            {
                Id = Guid.NewGuid(),
                destUserInfo = destUserInfo,
                message = notificationMessage,
                createdAt = DateTime.Now
            };
        }
    }
}

using CoreLib.Common;
using CoreLib.Exceptions;
using CoreLib.Interfaces;
using NotificationApi.Domain.Entities;
using NotificationApi.Domain.Interfaces;
using NotificationApi.Services.Interfaces;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace NotificationApi.Services.Implementations
{
    public class CreateNotification : ICreateNotification
    {
        private IGetUserInfo _getUserInfo;
        private IMapper<UserInfoDtoResponse, UserInfo> _mapper;

        public CreateNotification(IGetUserInfo getUserInfo, IMapper<UserInfoDtoResponse, UserInfo> mapper)
        {
            _getUserInfo = getUserInfo;
            _mapper = mapper;
        }

        public async Task<Notification> createNotification(Guid? srcUserId, Guid destUserId, string notificationType, string message)
        {
            NotificationType type = (NotificationType) Enum.Parse(typeof(NotificationType), notificationType);

            UserInfo? srcUserInfo = null;
            UserInfo? destUserInfo = _mapper.map(await _getUserInfo.getUserInfo(destUserId));

            if (destUserInfo is null)
            {
                throw new ServiceException($"Адресат с id: {srcUserId} не найден!");
            }

            string notificationMessage;
            switch (type)
            {
                case NotificationType.USER_MESSAGE:
                    {
                        if (!srcUserId.HasValue)
                        {
                            throw new ArgumentException("Не могу отправить сообщение от пользователя с id: " + null);
                        }

                        srcUserInfo = _mapper.map(await _getUserInfo.getUserInfo(srcUserId.Value));

                        if (srcUserInfo is null)
                        {
                            throw new ServiceException($"Отправитель с id: {srcUserId} не найден!");
                        }

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

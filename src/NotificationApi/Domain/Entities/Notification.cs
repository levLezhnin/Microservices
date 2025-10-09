using CoreLib.Common;
using CoreLib.Dto;

namespace NotificationApi.Domain.Entities
{
    public record Notification : BaseEntityDal<Guid>
    {
        public UserInfoDto destUserInfo { get; set; }
        public string message { get; set; }
        public DateTime createdAt { get; set; }
    }
}

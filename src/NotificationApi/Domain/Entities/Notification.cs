using CoreLib.Common;

namespace NotificationApi.Domain.Entities
{
    public record Notification : BaseEntityDal<Guid>
    {
        public UserInfo destUserInfo { get; set; }
        public string message { get; set; }
        public DateTime createdAt { get; set; }
    }
}

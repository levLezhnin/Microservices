namespace CoreLib.Dto
{
    public record NotificationRequestDto
    {
        public Guid? srcUserId { get; init; }
        public Guid destUserId { get; init; }
        public string notificationType { get; init; }
        public string notificationMessage { get; init; }
    }
}

namespace NotificationApi.Infrastructure.Config
{
    public record RabbitMQConfig
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Virtual_Host { get; init; }
        public string Notification_Queue_Name { get; init; }
    }
}

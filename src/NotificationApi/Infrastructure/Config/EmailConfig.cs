namespace NotificationApi.Infrastructure.Config
{
    public record EmailConfig
    {
        public string SmtpServer { get; init; }
        public int Port { get; init; }
        public string SenderName { get; init; }
        public string SenderEmail { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
    }
}

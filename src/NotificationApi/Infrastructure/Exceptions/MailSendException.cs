namespace NotificationApi.Infrastructure.Exceptions
{
    public class MailSendException : Exception
    {
        public MailSendException() { }
        public MailSendException(string message) : base(message) { }
    }
}

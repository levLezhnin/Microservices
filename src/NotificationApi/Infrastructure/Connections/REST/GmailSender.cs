using Microsoft.Extensions.Options;
using NotificationApi.Domain.Entities;
using NotificationApi.Domain.Interfaces;
using NotificationApi.Infrastructure.Config;
using NotificationApi.Infrastructure.Exceptions;
using System.Net;
using System.Net.Mail;

namespace NotificationApi.Infrastructure.Connections.REST
{
    public class GmailSender : ISender
    {
        private readonly EmailConfig _emailConfig;
        private readonly SmtpClient _smtpClient;

        public GmailSender(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
            _smtpClient = new SmtpClient {
                Host = _emailConfig.SmtpServer,
                Port = _emailConfig.Port,
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };
        }

        public async Task sendNotificationAsync(Notification notification)
        {
            MailMessage mailMessage = createMailMessage(notification);
            await sendEmailMessageAsync(mailMessage);
        }

        private MailMessage createMailMessage(Notification notification)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.SenderEmail, _emailConfig.SenderName),
                Subject = "Уведомление от Ticket Service",
                Body = notification.message,
                IsBodyHtml = false
            };

            mailMessage.To.Add(notification.destUserInfo.email);
            return mailMessage;
        }

        private async Task sendEmailMessageAsync(MailMessage mailMessage)
        {
            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new MailSendException($"Не удалось отправить сообщение по почте: {ex.Message}");
            }
        }
    }
}

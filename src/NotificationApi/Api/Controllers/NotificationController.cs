using CoreLib.Dto;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.Services.Interfaces;

namespace NotificationApi.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class NotificationController
    {
        private readonly ISendNotification _sendNotification;

        public NotificationController(ISendNotification sendNotification)
        {
            _sendNotification = sendNotification;
        }

        [HttpPost("send")]
        public async Task sendNotification(NotificationRequestDto notificationRequestDto)
        {
            await _sendNotification.sendNotificationAsync(
                notificationRequestDto.srcUserId, 
                notificationRequestDto.destUserId,
                notificationRequestDto.notificationType,
                notificationRequestDto.notificationMessage
            );
        }
    }
}

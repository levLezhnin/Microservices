using CoreLib.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationApi.Infrastructure.Config;
using NotificationApi.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationApi.Infrastructure.Connections.Messaging;

public class RabbitMQConsumer : BackgroundService
{
    private RabbitMQConfig _rabbitMqConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IConnection _connection;
    private IModel _channel;

    public RabbitMQConsumer(IOptions<RabbitMQConfig> rabbitMqConfig, IServiceScopeFactory serviceScopeFactory)
    {
        _rabbitMqConfig = rabbitMqConfig.Value;
        _serviceScopeFactory = serviceScopeFactory;

        var factory = new ConnectionFactory { 
            HostName = _rabbitMqConfig.Host, 
            UserName = _rabbitMqConfig.Username,
            Password = _rabbitMqConfig.Password,
            Port = _rabbitMqConfig.Port,
            VirtualHost = _rabbitMqConfig.Virtual_Host
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _rabbitMqConfig.Notification_Queue_Name, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            ISendNotification sendNotification = scope.ServiceProvider.GetRequiredService<ISendNotification>();

            string content = Encoding.UTF8.GetString(ea.Body.ToArray());
            
            NotificationRequestDto notificationReq = JsonSerializer.Deserialize<NotificationRequestDto>(content);

            await sendNotification.sendNotificationAsync(
                notificationReq.srcUserId,
                notificationReq.destUserId,
                notificationReq.notificationType,
                notificationReq.notificationMessage
            );

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_rabbitMqConfig.Notification_Queue_Name, false, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
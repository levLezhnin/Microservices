using CoreLib.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;

    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQConsumer(IOptions<RabbitMQConfig> rabbitMqConfig, IServiceScopeFactory serviceScopeFactory)
    {
        _rabbitMqConfig = rabbitMqConfig.Value;
        _serviceScopeFactory = serviceScopeFactory;
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().AddDebug().SetMinimumLevel(LogLevel.Debug);
        });

        _logger = loggerFactory.CreateLogger<RabbitMQConsumer>();

        var factory = new ConnectionFactory { 
            HostName = _rabbitMqConfig.Host, 
            UserName = _rabbitMqConfig.Username,
            Password = _rabbitMqConfig.Password,
            Port = _rabbitMqConfig.Port,
            VirtualHost = _rabbitMqConfig.Virtual_Host
        };
        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
        _channel.QueueDeclareAsync(queue: _rabbitMqConfig.Notification_Queue_Name, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Пошёл читать сообщения");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            try
            {
                _logger.LogInformation("Получил сообщение");

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

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            } catch (Exception ex)
            {
                _logger.LogInformation($"Ошибка! {ex.Message}");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }
        };

        await _channel.BasicConsumeAsync(_rabbitMqConfig.Notification_Queue_Name, false, consumer);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Больше не читаю сообщения");
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _channel.CloseAsync();
        _connection.CloseAsync();
        base.Dispose();
    }
}
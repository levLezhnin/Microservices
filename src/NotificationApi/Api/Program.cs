using DotNetEnv;
using NotificationApi.Infrastructure.Config;
using NotificationApi.Infrastructure;
using NotificationApi.Services;
using NotificationApi.Infrastructure.Connections.Messaging;

namespace NotificationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Load();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<RabbitMQConfig>(
                builder.Configuration.GetSection("RABBITMQ")
            );

            builder.Services.Configure<EmailConfig>(
                builder.Configuration.GetSection("APP_EMAIL")
            );

            builder.Services.TryAddServices();
            builder.Services.TryAddInfra();

            builder.Services.AddHostedService<RabbitMQConsumer>();

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}

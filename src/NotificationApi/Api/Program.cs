using CoreLib.HttpLogic;
using CoreLib.TraceIdLogic;
using DotNetEnv;
using NotificationApi.Infrastructure;
using NotificationApi.Infrastructure.Config;
using NotificationApi.Infrastructure.Connections.Messaging;
using NotificationApi.Services;
using Serilog;

namespace NotificationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Load();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<RabbitMQConfig>(
                builder.Configuration.GetSection("RABBITMQ")
            );

            builder.Services.Configure<EmailConfig>(options =>
                {
                    options.SmtpServer = builder.Configuration["APP_EMAIL_SMTP_SERVER"] ?? "";
                    options.Port = int.TryParse(builder.Configuration["APP_EMAIL_PORT"], out var port) ? port : 0;
                    options.SenderName = builder.Configuration["APP_EMAIL_SENDER_NAME"] ?? "";
                    options.SenderEmail = builder.Configuration["APP_EMAIL_SENDER_EMAIL"] ?? "";
                    options.Username = builder.Configuration["APP_EMAIL_USERNAME"] ?? "";
                    options.Password = builder.Configuration["APP_EMAIL_PASSWORD"] ?? "";
                }
            );

            builder.Services.TryAddServices();
            builder.Services.TryAddInfra();
            builder.Services.AddHttpRequestService();
            builder.Services.TryAddTraceId();

            builder.Services.AddHostedService<RabbitMQConsumer>();

            builder.Services.AddControllers();

            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console(
                                outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Properties}{NewLine}{Exception}"
                            )
                            .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.UseTraceId();

            app.Run();
        }
    }
}

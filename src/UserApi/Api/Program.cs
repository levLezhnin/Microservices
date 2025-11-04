using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserApi.Api.Exceptions;
using UserApi.Dal;
using UserApi.Dal.Config;
using UserApi.Logic;
using UserApi.Logic.Saga;

namespace UserApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddDbContext<UserDbContext>(
                    options => options.UseNpgsql(
                        builder.Configuration.GetConnectionString("UserDatabase")
                    )
                )
                .AddScoped<DbContext, UserDbContext>();

            builder.Services.TryAddDal();
            builder.Services.TryAddLogic();
            builder.Services.TryAddController();
            
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<SupportAgentUpdateEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("rabbit_user");
                        h.Password("rabbit_password");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}

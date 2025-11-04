using Api.Exceptions;
using CoreLib.HttpLogic;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Messaging.Saga;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;
using Services.Interfaces;
using Services.Messaging.Saga;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddDbContext<TicketDbContext>(
                    options => options.UseNpgsql(
                        builder.Configuration.GetConnectionString("TicketDatabase")
                    )
                )
                .AddScoped<DbContext, TicketDbContext>();

            builder.Services.TryAddInfra();
            builder.Services.TryAddServices();
            builder.Services.AddHttpRequestService();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<StartTicketCreationConsumer>();
                x.AddSagaStateMachine<TicketSaga, TicketSagaState>()
                    .InMemoryRepository();

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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers(); 
            
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

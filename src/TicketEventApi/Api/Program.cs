using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Messaging.Saga;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Services;

namespace TicketEventApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<TicketEventDbContext>(
                options => options.UseMongoDB(
                    builder.Configuration.GetConnectionString("TicketEventsDatabase")
                )
            )
            .AddScoped<DbContext, TicketEventDbContext>();

            builder.Services.TryAddServices();
            builder.Services.TryAddInfra();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<TicketCreatedEventConsumer>();

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

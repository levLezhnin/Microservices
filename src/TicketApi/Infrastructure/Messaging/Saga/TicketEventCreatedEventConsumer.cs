using Contracts.Messages;
using Contracts.SupportAgentAssignMessages.Init;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Saga
{
    public class TicketEventCreatedEventConsumer : IConsumer<TicketEventCreated>
    {
        private readonly ILogger<TicketEventCreatedEventConsumer> _logger;

        public TicketEventCreatedEventConsumer(ILogger<TicketEventCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TicketEventCreated> context)
        {
            try
            {
                _logger.LogInformation("Начинаю назначение созданного тикета...");
                await context.Publish(new StartTicketAssign
                {
                    CorrelationId = Guid.NewGuid(),
                    TicketId = context.Message.TicketId
                });
            } 
            catch (Exception ex)
            {
                _logger.LogWarning($"Что-то пошло не так: {ex.Message}");
            }
        }
    }
}

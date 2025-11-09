using Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Infrastructure.Messaging.Saga
{
    public class TicketEventCreationFailedConsumer : IConsumer<TicketEventCreationFailed>
    {
        private readonly IDeleteTicket _deleteTicket;
        private ILogger<TicketEventCreationFailedConsumer> _logger;


        public TicketEventCreationFailedConsumer(IDeleteTicket deleteTicket, ILogger<TicketEventCreationFailedConsumer> logger)
        {
            _deleteTicket = deleteTicket;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TicketEventCreationFailed> context)
        {
            _logger.LogInformation("Откатываю создание тикета...");
            await _deleteTicket.deleteTicket(context.Message.TicketId);
        }
    }
}

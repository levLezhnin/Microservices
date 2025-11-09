using Contracts.Messages;
using Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Infrastructure.Messaging.Saga
{
    public class StartTicketCreationConsumer : IConsumer<StartTicketCreation>
    {
        private readonly ICreateTicket _createTicket;
        private readonly IDeleteTicket _deleteTicket;
        private ILogger<StartTicketCreationConsumer> _logger;

        public StartTicketCreationConsumer(ICreateTicket createTicket, IDeleteTicket deleteTicket, ILogger<StartTicketCreationConsumer> logger)
        {
            _createTicket = createTicket;
            _deleteTicket = deleteTicket;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StartTicketCreation> context)
        {
            Guid creatorId = context.Message.CreatorId;
            string title = context.Message.Title;
            string? description = context.Message.Description;

            Ticket ticket;// = null;
            try
            {
                _logger.LogInformation("Создаю тикет...");
                ticket = await _createTicket.createAsync(creatorId, title, description);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Тикет не был создан: {ex.Message}");

                await context.Publish(new TicketCreationFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    Reason = ex.Message
                });
                throw;
            }

            try
            {
                _logger.LogInformation("Отправляю событие TicketCreated...");

                await context.Publish(new TicketCreated
                {
                    CorrelationId = context.Message.CorrelationId,
                    TicketId = ticket.Id,
                    CreatorId = creatorId,
                    Title = title,
                    CreatedAt = DateTime.UtcNow
                });
            } catch (Exception ex)
            {
                _logger.LogWarning($"Не смог отправить событие: {ex.Message}");

                _logger.LogInformation("Откатываю создание тикета");
                await _deleteTicket.deleteTicket(ticket.Id);

                _logger.LogInformation("Отправляю событие TicketCreationFailed...");
                await context.Publish(new TicketCreationFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    Reason = ex.Message
                });
                throw;
            }
        }
    }
}

using Contracts.Messages;
using CoreLib.Common;
using Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Services.Interfaces;

namespace Infrastructure.Messaging.Saga
{
    public class TicketCreatedEventConsumer : IConsumer<TicketCreated>
    {
        private readonly ICreateTicketEventRabbit _createTicketEvent;
        private readonly IDeleteTicketEvent _deleteTicketEvent;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Random _random;
        private readonly ILogger<TicketCreatedEventConsumer> _logger;

        public TicketCreatedEventConsumer(ILogger<TicketCreatedEventConsumer> logger, ICreateTicketEventRabbit createTicketEvent, IDeleteTicketEvent deleteTicketEvent, IPublishEndpoint publishEndpoint)
        {
            _createTicketEvent = createTicketEvent;
            _deleteTicketEvent = deleteTicketEvent;
            _publishEndpoint = publishEndpoint;
            _random = new Random();
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TicketCreated> context)
        {
            Guid correlationId = context.Message.CorrelationId;
            Guid ticketId = context.Message.TicketId;
            ObjectId ticketEventId = new ObjectId();
            try
            {
                _logger.LogInformation("Создаю событие тикета...");
                //if (_random.Next(0, 2) == 1)
                //{
                //    throw new Exception("Random stuff happened");
                //}

                TicketEvent ticketEvent = await _createTicketEvent.createEventAsync(
                    EventType.Created,
                    ticketId,
                    $"Пользователь с Id: {context.Message.CreatorId} создал тикет: {context.Message.Title}"
                );
                ticketEventId = ticketEvent.Id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Событие не было создано: {ex.Message}");
                await _publishEndpoint.Publish(new TicketEventCreationFailed
                {
                    CorrelationId = correlationId,
                    TicketId = ticketId,
                    Reason = ex.Message
                });
                throw;
            }

            try
            {
                _logger.LogInformation("Отправляю событие TicketEventCreated...");
                await _publishEndpoint.Publish(new TicketEventCreated
                {
                    CorrelationId = correlationId,
                    TicketId = context.Message.TicketId,
                    EventId = ticketEventId.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Событие не было отправлено: {ex.Message}");
                await _deleteTicketEvent.deleteTicketEvent(ticketEventId);
                await _publishEndpoint.Publish(new TicketEventCreationFailed
                {
                    CorrelationId = correlationId,
                    TicketId = ticketId,
                    Reason = ex.Message
                });
                throw;
            }
        }
    }
}

using CoreLib.Common;
using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class CreateTicketEventRabbit : ICreateTicketEventRabbit
    {
        private readonly ITicketEventRepository _ticketEventRepository;

        public CreateTicketEventRabbit(ITicketEventRepository ticketEventRepository)
        {
            _ticketEventRepository = ticketEventRepository;
        }

        public async Task<TicketEvent> createEventAsync(EventType eventType, Guid ticketId, string description)
        {
            TicketEvent ticketEvent = new TicketEvent
            {
                eventType = eventType,
                ticketId = ticketId,
                description = description,
                createdAt = DateTime.UtcNow
            };
            return await _ticketEventRepository.insert(ticketEvent);
        }
    }
}

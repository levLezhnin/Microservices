using CoreLib.Common;
using Domain.Entities;

namespace Services.Interfaces
{
    public interface ICreateTicketEventRabbit
    {
        Task<TicketEvent> createEventAsync(EventType eventType, Guid ticketId, string description);
    }
}

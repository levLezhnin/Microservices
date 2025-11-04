using CoreLib.Common;
using Domain.Entities;

namespace Services.Interfaces
{
    public interface IChangeTicketStatus
    {
        Task<Ticket> changeTicketStatusAsync(Guid ticketId, TicketStatuses status);
    }
}

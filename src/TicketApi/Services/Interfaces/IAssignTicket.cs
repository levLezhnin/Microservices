using Domain.Entities;

namespace Services.Interfaces
{
    public interface IAssignTicket
    {
        Task<Ticket> assignToSupportAgent(Guid ticketId, Guid supportAgentId);
    }
}

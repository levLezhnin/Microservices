using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class RemoveAgent : IRemoveAgent
    {
        private readonly ITicketRepository _ticketRepository;

        public RemoveAgent(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task removeAgent(Guid ticketId)
        {
            Ticket ticket = await _ticketRepository.findByIdOrThrowAsync(ticketId);
            ticket.assignedSupportAgentId = null;
            await _ticketRepository.update(ticket);
        }
    }
}

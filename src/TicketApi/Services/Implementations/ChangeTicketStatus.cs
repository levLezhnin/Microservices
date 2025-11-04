using CoreLib.Common;
using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ChangeTicketStatus : IChangeTicketStatus
    {
        private readonly ITicketRepository _ticketRepository;

        public ChangeTicketStatus(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> changeTicketStatusAsync(Guid ticketId, TicketStatuses status)
        {
            Ticket ticket = await _ticketRepository.findByIdOrThrowAsync(ticketId);
            ticket.status = status;
            return await _ticketRepository.update(ticket);
        }
    }
}

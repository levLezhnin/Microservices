using Domain.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class DeleteTicket : IDeleteTicket
    {
        private readonly ITicketRepository _ticketRepository;

        public DeleteTicket(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> deleteTicket(Guid ticketId)
        {
            try
            {
                return await _ticketRepository.deleteByIdAsync(ticketId);
            } catch
            {
                return false;
            }
        }
    }
}

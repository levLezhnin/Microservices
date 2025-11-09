using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class GetTicket : IGetTicket
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicket(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> getTicketById(Guid id)
        {
            return await _ticketRepository.findByIdOrThrowAsync(id);
        }
    }
}

using Domain.Entities;

namespace Services.Interfaces
{
    public interface IGetTicket
    {
        Task<Ticket> getTicketById(Guid id);
    }
}

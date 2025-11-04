using CoreLib.Interfaces;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITicketRepository : ICrudRepository<Ticket, Guid>
    {
    }
}

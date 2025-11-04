using Domain.Entities;

namespace Services.Interfaces
{
    public interface ICreateTicket
    {
        Task<Ticket> createAsync(Guid creatorId, string title, string? description);
    }
}

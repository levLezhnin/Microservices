namespace Services.Interfaces
{
    public interface IDeleteTicket
    {
        Task<bool> deleteTicket(Guid ticketId);
    }
}

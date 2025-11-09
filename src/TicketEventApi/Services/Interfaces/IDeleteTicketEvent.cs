using MongoDB.Bson;

namespace Services.Interfaces
{
    public interface IDeleteTicketEvent
    {
        Task<bool> deleteTicketEvent(ObjectId ticketEventId);
    }
}

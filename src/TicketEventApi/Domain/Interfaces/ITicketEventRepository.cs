using CoreLib.Interfaces;
using Domain.Entities;
using MongoDB.Bson;

namespace Domain.Interfaces
{
    public interface ITicketEventRepository : ICrudRepository<TicketEvent, ObjectId>
    {
    }
}

using CoreLib.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Infrastructure.Repository
{
    public class TicketEventRepository : ITicketEventRepository
    {
        private readonly TicketEventDbContext _ticketEventDbContext;
        private readonly DbSet<TicketEvent> _ticketEventsDbSet;

        public TicketEventRepository(TicketEventDbContext ticketEventDbContext)
        {
            _ticketEventDbContext = ticketEventDbContext;
            _ticketEventsDbSet = _ticketEventDbContext.Set<TicketEvent>();
        }

        public async Task<TicketEvent> insert(TicketEvent entity)
        {
            await _ticketEventDbContext.AddAsync(entity);
            await _ticketEventDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TicketEvent> update(TicketEvent entity)
        {
            _ticketEventsDbSet.Update(entity);
            await _ticketEventDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TicketEvent?> findByIdAsync(ObjectId id)
        {
            return await _ticketEventsDbSet.FindAsync(id);
        }

        public async Task<TicketEvent> findByIdOrThrowAsync(ObjectId id)
        {
            return await _ticketEventsDbSet.FindAsync(id) ?? 
                throw new EntityNotFoundException($"Событие с id: {id} не найдено!");
        }

        public async Task<bool> deleteByIdAsync(ObjectId id)
        {
            var entity = await findByIdOrThrowAsync(id);

            _ticketEventsDbSet.Remove(entity);
            await _ticketEventDbContext.SaveChangesAsync();

            return true;
        }
    }
}

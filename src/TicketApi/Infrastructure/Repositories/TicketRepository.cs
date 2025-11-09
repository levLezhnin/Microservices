using CoreLib.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketDbContext _dbContext;
        private readonly DbSet<Ticket> _dbSet;

        public TicketRepository(TicketDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Ticket>();
        }

        public async Task<Ticket> insert(Ticket entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Ticket> update(Ticket entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Ticket?> findByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Ticket> findByIdOrThrowAsync(Guid id)
        {
            return await _dbSet.FindAsync(id) ??
                throw new EntityNotFoundException($"Тикет с id: {id} не найден!");
        }

        public async Task<bool> deleteByIdAsync(Guid id)
        {
            var entity = await findByIdOrThrowAsync(id);

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}

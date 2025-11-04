using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Infrastructure.Data
{
    public class TicketEventDbContext : DbContext
    {
        public TicketEventDbContext(DbContextOptions<TicketEventDbContext> options) : base(options) { }

        public DbSet<TicketEvent> ticketEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TicketEvent>().ToCollection("ticket_events");
        }
    }
}

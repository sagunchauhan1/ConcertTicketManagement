using Microsoft.EntityFrameworkCore;
using EventService.Models;

namespace EventService.Data;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }
    public DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .OwnsMany(e => e.TicketTypes, tt =>
            {
                tt.WithOwner().HasForeignKey("EventId");
                tt.Property<Guid>("Id");
                tt.HasKey("Id");
            });
    }
}

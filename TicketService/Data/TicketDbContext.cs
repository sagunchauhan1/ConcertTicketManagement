using Microsoft.EntityFrameworkCore;
using TicketService.Models;

namespace TicketService.Data;

public class TicketDbContext : DbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

    public DbSet<TicketReservation> Reservations => Set<TicketReservation>();
}

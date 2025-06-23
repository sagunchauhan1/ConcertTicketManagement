using Microsoft.EntityFrameworkCore;
using VenueService.Models;

namespace VenueService.Data;

public class VenueDbContext : DbContext
{
    public VenueDbContext(DbContextOptions<VenueDbContext> options) : base(options) { }

    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<VenueReservation> Reservations => Set<VenueReservation>();
}

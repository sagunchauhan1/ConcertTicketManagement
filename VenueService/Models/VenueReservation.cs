namespace VenueService.Models;

public class VenueReservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string VenueId { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public int ReservedSeats { get; set; }
}

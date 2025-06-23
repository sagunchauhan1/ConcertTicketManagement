namespace TicketService.Models;

public class VenueCapacityResponse
{
    public string VenueId { get; set; } = string.Empty;
    public int TotalCapacity { get; set; }
    public int Reserved { get; set; }
    public int Available { get; set; }
}

namespace VenueService.Models;

public class Venue
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

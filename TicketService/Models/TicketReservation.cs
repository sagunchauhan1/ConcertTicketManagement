namespace TicketService.Models;

public class TicketReservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Guid TicketTypeId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
    public bool IsPurchased { get; set; } = false;
}

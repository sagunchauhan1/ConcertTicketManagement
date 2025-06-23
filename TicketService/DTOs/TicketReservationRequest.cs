namespace TicketService.DTOs;

public class TicketReservationRequest
{
    public Guid EventId { get; set; }
    public Guid TicketTypeId { get; set; }
    public int Quantity { get; set; }
}
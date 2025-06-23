using EventService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TicketService.Data;
using TicketService.DTOs;
using TicketService.Models;

namespace TicketService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(TicketDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
{
    private readonly TicketDbContext _context = context;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve([FromBody] TicketReservationRequest request)
    {
        // 1. Check ticket type availability via EventService
        var eventClient = _httpClientFactory.CreateClient("EventService");
        var eventResponse = await eventClient.GetAsync($"/api/events/{request.EventId}");

        if (!eventResponse.IsSuccessStatusCode)
            return BadRequest("Event not found");

        var eventData = await eventResponse.Content.ReadFromJsonAsync<Event>();

        var ticketType = eventData?.TicketTypes.FirstOrDefault(t => t.Id == request.TicketTypeId);
        if (ticketType == null || ticketType.AvailableQuantity < request.Quantity)
            return BadRequest("Not enough tickets");

        // 2. Check capacity with VenueService
        var venueClient = _httpClientFactory.CreateClient("VenueService");
        var capacityResponse = await venueClient.GetAsync($"/api/venues/{eventData.VenueId}/capacity");

        if (!capacityResponse.IsSuccessStatusCode)
            return BadRequest("Venue not found");

        var capacityData = await capacityResponse.Content.ReadFromJsonAsync<VenueCapacityResponse>();
        if (capacityData!.Available < request.Quantity)
            return BadRequest("Venue has no enough seats");

        // 3. Reserve tickets
        var reservation = new TicketReservation
        {
            EventId = request.EventId,
            TicketTypeId = request.TicketTypeId,
            Quantity = request.Quantity
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        // 4. Inform VenueService (optional)
        await venueClient.PostAsync($"/api/venues/{eventData.VenueId}/reserve?eventId={request.EventId}&quantity={request.Quantity}", null);

        return Ok(new { reservationId = reservation.Id });
    }

    // Purchase Ticket
    [HttpPost("purchase/{id}")]
    public async Task<IActionResult> Purchase(Guid id)
    {
        var res = await _context.Reservations.FindAsync(id);
        if (res == null) return NotFound();
        res.IsPurchased = true;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Purchased", reservationId = id });
    }

    // Cancel a reservation
    [HttpDelete("cancel/{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var res = await _context.Reservations.FindAsync(id);
        if (res == null)
            return NotFound(new { message = "Reservation not found" });

        if (res.IsPurchased)
            return BadRequest(new { message = "Cannot cancel a purchased reservation" });

        _context.Reservations.Remove(res);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Reservation cancelled", reservationId = id });
    }

    // View Reservation
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await _context.Reservations.FindAsync(id);
        return res is null ? NotFound() : Ok(res);
    }
}

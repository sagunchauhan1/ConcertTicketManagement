using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VenueService.Data;
using VenueService.Models;

namespace VenueService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController(VenueDbContext context) : ControllerBase
{
    private readonly VenueDbContext _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateVenue([FromBody] Venue venue)
    {
        _context.Venues.Add(venue);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetVenue), new { id = venue.Id }, venue);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVenue(string id)
    {
        var venue = await _context.Venues.FindAsync(id);
        return venue is null ? NotFound() : Ok(venue);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var venues = await _context.Venues.ToListAsync();
        return Ok(venues);
    }

    [HttpPost("{venueId}/reserve")]
    public async Task<IActionResult> ReserveSeats(string venueId, [FromQuery] Guid eventId, [FromQuery] int quantity)
    {
        var venue = await _context.Venues.FindAsync(venueId);
        if (venue is null) return NotFound("Venue not found");

        var reserved = await _context.Reservations
            .Where(r => r.VenueId == venueId)
            .SumAsync(r => r.ReservedSeats);

        if (reserved + quantity > venue.Capacity)
            return BadRequest("Not enough capacity");

        var reservation = new VenueReservation
        {
            VenueId = venueId,
            EventId = eventId,
            ReservedSeats = quantity
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Seats reserved", reservation.Id });
    }

    [HttpGet("{id}/capacity")]
    public async Task<IActionResult> GetVenueCapacity(string id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue is null) return NotFound();

        var reserved = await _context.Reservations
            .Where(r => r.VenueId == id)
            .SumAsync(r => r.ReservedSeats);

        return Ok(new
        {
            VenueId = venue.Id,
            VenueName = venue.Name,
            TotalCapacity = venue.Capacity,
            Reserved = reserved,
            Available = venue.Capacity - reserved
        });
    }
}

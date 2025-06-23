using EventService.Data;
using EventService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(EventDbContext context) : ControllerBase
{
    private readonly EventDbContext _context = context;

    // Create Event
    [HttpPost]
    public async Task<IActionResult> Create(Event ev)
    {
        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = ev.Id }, ev);
    }

    // Update Event
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Event updated)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();

        ev.Name = updated.Name;
        ev.Description = updated.Description;
        ev.Date = updated.Date;
        ev.VenueId = updated.VenueId;
        ev.TicketTypes = updated.TicketTypes;

        await _context.SaveChangesAsync();
        return Ok(ev);
    }

    // Get Event
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var ev = await _context.Events.Include(e => e.TicketTypes).FirstOrDefaultAsync(e => e.Id == id);
        return ev is null ? NotFound() : Ok(ev);
    }

    // View Ticket Availability
    [HttpGet("{id}/availability")]
    public async Task<IActionResult> GetAvailability(Guid id)
    {
        var ev = await _context.Events.Include(e => e.TicketTypes).FirstOrDefaultAsync(e => e.Id == id);
        return ev is null ? NotFound() : Ok(ev.TicketTypes.Select(t => new {
            t.Name,
            t.Price,
            t.AvailableQuantity
        }));
    }
}
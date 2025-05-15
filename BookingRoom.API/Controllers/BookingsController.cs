// Controllers/BookingsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingRoom.Infrastructure.Data;
using BookingRoom.Core.Models;

namespace BookingRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound();
        }

        return booking;
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
    {
        // Validate room availability
        var isRoomAvailable = await IsRoomAvailable(booking.RoomId, booking.StartTime, booking.EndTime);
        if (!isRoomAvailable)
        {
            return BadRequest("Room is not available for the selected time period");
        }

        booking.CreatedAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;
        booking.Status = BookingStatus.Pending;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromQuery] BookingStatus status, [FromQuery] string? adminComment)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        booking.Status = status;
        booking.AdminComment = adminComment;
        booking.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetUserBookings(int userId)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetRoomBookings(int roomId)
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Where(b => b.RoomId == roomId)
            .ToListAsync();
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByStatus(BookingStatus status)
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .Where(b => b.Status == status)
            .ToListAsync();
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByDateRange(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        return await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .Where(b => b.StartTime >= start && b.EndTime <= end)
            .ToListAsync();
    }

    private bool BookingExists(int id)
    {
        return _context.Bookings.Any(e => e.Id == id);
    }

    private async Task<bool> IsRoomAvailable(int roomId, DateTime startTime, DateTime endTime)
    {
        return !await _context.Bookings
            .AnyAsync(b => b.RoomId == roomId &&
                          b.Status == BookingStatus.Approved &&
                          ((startTime >= b.StartTime && startTime < b.EndTime) ||
                           (endTime > b.StartTime && endTime <= b.EndTime) ||
                           (startTime <= b.StartTime && endTime >= b.EndTime)));
    }
}
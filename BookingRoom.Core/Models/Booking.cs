// Models/Booking.cs
namespace BookingRoom.Core.Models;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string? AdminComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public Room Room { get; set; } = null!;
}

public enum BookingStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}
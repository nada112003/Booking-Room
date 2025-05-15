// Models/Room.cs
namespace BookingRoom.Core.Models;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public RoomType RoomType { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public enum RoomType
{
    LectureHall,
    Laboratory,
    ConferenceRoom,
    StudyRoom
}
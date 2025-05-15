// Models/User.cs
namespace BookingRoom.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    public UserStatus Status { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public enum UserRole
{
    Student,
    Staff,
    Admin
}

public enum UserStatus
{
    Active,
    Suspended,
    Deactivated
}
namespace HotelService.Models;

public class Reservation
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Guest Guest { get; set; } = null!;
    public Room Room { get; set; } = null!;
}

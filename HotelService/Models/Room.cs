namespace HotelService.Models;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public decimal Rate { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Navigation properties
    public RoomType RoomType { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

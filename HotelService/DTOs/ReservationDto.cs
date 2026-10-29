using HotelService.Models;

namespace HotelService.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

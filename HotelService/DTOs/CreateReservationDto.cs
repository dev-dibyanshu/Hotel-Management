using System.ComponentModel.DataAnnotations;

namespace HotelService.DTOs;

public class CreateReservationDto
{
    [Required]
    public int GuestId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }
}

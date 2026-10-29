using System.ComponentModel.DataAnnotations;

namespace HotelService.DTOs;

public class CreateRoomDto
{
    [Required]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    public int RoomTypeId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
    public decimal Rate { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace HotelService.DTOs;

public class CreateRoomTypeDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 10)]
    public int Capacity { get; set; }
}

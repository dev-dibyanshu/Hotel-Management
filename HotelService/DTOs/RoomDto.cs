namespace HotelService.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsAvailable { get; set; }
}

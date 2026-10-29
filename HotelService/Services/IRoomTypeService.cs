using HotelService.DTOs;

namespace HotelService.Services;

public interface IRoomTypeService
{
    Task<IEnumerable<RoomTypeDto>> GetAllAsync();
    Task<RoomTypeDto?> GetByIdAsync(int id);
    Task<RoomTypeDto> CreateAsync(CreateRoomTypeDto createRoomTypeDto);
    Task<RoomTypeDto?> UpdateAsync(int id, CreateRoomTypeDto createRoomTypeDto);
    Task<bool> DeleteAsync(int id);
}

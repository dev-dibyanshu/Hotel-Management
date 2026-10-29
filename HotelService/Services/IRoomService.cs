using HotelService.DTOs;

namespace HotelService.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync();
    Task<RoomDto?> GetByIdAsync(int id);
    Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync();
    Task<RoomDto?> CreateAsync(CreateRoomDto createRoomDto);
    Task<RoomDto?> UpdateAsync(int id, CreateRoomDto createRoomDto);
    Task<bool> DeleteAsync(int id);
}

using HotelService.Models;

namespace HotelService.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync();
    Task<Room> CreateAsync(Room room);
    Task<Room?> UpdateAsync(Room room);
    Task<bool> DeleteAsync(int id);
}

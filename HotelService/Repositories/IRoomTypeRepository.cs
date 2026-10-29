using HotelService.Models;

namespace HotelService.Repositories;

public interface IRoomTypeRepository
{
    Task<IEnumerable<RoomType>> GetAllAsync();
    Task<RoomType?> GetByIdAsync(int id);
    Task<RoomType> CreateAsync(RoomType roomType);
    Task<RoomType?> UpdateAsync(RoomType roomType);
    Task<bool> DeleteAsync(int id);
}

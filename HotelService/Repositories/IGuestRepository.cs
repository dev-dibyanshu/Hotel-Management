using HotelService.Models;

namespace HotelService.Repositories;

public interface IGuestRepository
{
    Task<IEnumerable<Guest>> GetAllAsync();
    Task<Guest?> GetByIdAsync(int id);
    Task<Guest> CreateAsync(Guest guest);
    Task<Guest?> UpdateAsync(Guest guest);
    Task<bool> DeleteAsync(int id);
}

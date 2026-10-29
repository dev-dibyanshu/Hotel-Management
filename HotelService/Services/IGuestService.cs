using HotelService.DTOs;

namespace HotelService.Services;

public interface IGuestService
{
    Task<IEnumerable<GuestDto>> GetAllAsync();
    Task<GuestDto?> GetByIdAsync(int id);
    Task<GuestDto> CreateAsync(CreateGuestDto createGuestDto);
    Task<GuestDto?> UpdateAsync(int id, CreateGuestDto createGuestDto);
    Task<bool> DeleteAsync(int id);
}

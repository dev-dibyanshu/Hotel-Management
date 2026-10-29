using HotelService.DTOs;

namespace HotelService.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllAsync();
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<ReservationDto?> CreateAsync(CreateReservationDto createReservationDto);
    Task<ReservationDto?> UpdateAsync(int id, CreateReservationDto createReservationDto);
    Task<bool> CancelAsync(int id);
    Task<bool> DeleteAsync(int id);
}

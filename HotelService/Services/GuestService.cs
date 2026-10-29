using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;

namespace HotelService.Services;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;

    public GuestService(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<IEnumerable<GuestDto>> GetAllAsync()
    {
        var guests = await _guestRepository.GetAllAsync();
        return guests.Select(g => new GuestDto
        {
            Id = g.Id,
            FirstName = g.FirstName,
            LastName = g.LastName,
            Email = g.Email,
            Phone = g.Phone,
            CreatedAt = g.CreatedAt
        });
    }

    public async Task<GuestDto?> GetByIdAsync(int id)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        if (guest == null)
        {
            return null;
        }

        return new GuestDto
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            Phone = guest.Phone,
            CreatedAt = guest.CreatedAt
        };
    }

    public async Task<GuestDto> CreateAsync(CreateGuestDto createGuestDto)
    {
        var guest = new Guest
        {
            FirstName = createGuestDto.FirstName,
            LastName = createGuestDto.LastName,
            Email = createGuestDto.Email,
            Phone = createGuestDto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        var createdGuest = await _guestRepository.CreateAsync(guest);

        return new GuestDto
        {
            Id = createdGuest.Id,
            FirstName = createdGuest.FirstName,
            LastName = createdGuest.LastName,
            Email = createdGuest.Email,
            Phone = createdGuest.Phone,
            CreatedAt = createdGuest.CreatedAt
        };
    }

    public async Task<GuestDto?> UpdateAsync(int id, CreateGuestDto createGuestDto)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        if (guest == null)
        {
            return null;
        }

        guest.FirstName = createGuestDto.FirstName;
        guest.LastName = createGuestDto.LastName;
        guest.Email = createGuestDto.Email;
        guest.Phone = createGuestDto.Phone;

        var updatedGuest = await _guestRepository.UpdateAsync(guest);

        return new GuestDto
        {
            Id = updatedGuest!.Id,
            FirstName = updatedGuest.FirstName,
            LastName = updatedGuest.LastName,
            Email = updatedGuest.Email,
            Phone = updatedGuest.Phone,
            CreatedAt = updatedGuest.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _guestRepository.DeleteAsync(id);
    }
}

using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;

namespace HotelService.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IRoomRepository _roomRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IGuestRepository guestRepository,
        IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _guestRepository = guestRepository;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        var reservations = await _reservationRepository.GetAllAsync();
        return reservations.Select(r => new ReservationDto
        {
            Id = r.Id,
            GuestId = r.GuestId,
            GuestName = $"{r.Guest.FirstName} {r.Guest.LastName}",
            RoomId = r.RoomId,
            RoomNumber = r.Room.RoomNumber,
            CheckInDate = r.CheckInDate,
            CheckOutDate = r.CheckOutDate,
            Status = r.Status,
            CreatedAt = r.CreatedAt
        });
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            return null;
        }

        return new ReservationDto
        {
            Id = reservation.Id,
            GuestId = reservation.GuestId,
            GuestName = $"{reservation.Guest.FirstName} {reservation.Guest.LastName}",
            RoomId = reservation.RoomId,
            RoomNumber = reservation.Room.RoomNumber,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            Status = reservation.Status,
            CreatedAt = reservation.CreatedAt
        };
    }

    public async Task<ReservationDto?> CreateAsync(CreateReservationDto createReservationDto)
    {
        // Validate check-out is after check-in
        if (createReservationDto.CheckOutDate <= createReservationDto.CheckInDate)
        {
            return null;
        }

        // Check if guest exists
        var guest = await _guestRepository.GetByIdAsync(createReservationDto.GuestId);
        if (guest == null)
        {
            return null;
        }

        // Check if room exists
        var room = await _roomRepository.GetByIdAsync(createReservationDto.RoomId);
        if (room == null)
        {
            return null;
        }

        // Check if room is available
        if (!room.IsAvailable)
        {
            return null;
        }

        // Create reservation
        var reservation = new Reservation
        {
            GuestId = createReservationDto.GuestId,
            RoomId = createReservationDto.RoomId,
            CheckInDate = createReservationDto.CheckInDate,
            CheckOutDate = createReservationDto.CheckOutDate,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var createdReservation = await _reservationRepository.CreateAsync(reservation);

        // Mark room as unavailable
        room.IsAvailable = false;
        await _roomRepository.UpdateAsync(room);

        // Load the full reservation with navigation properties
        var loadedReservation = await _reservationRepository.GetByIdAsync(createdReservation.Id);

        return new ReservationDto
        {
            Id = loadedReservation!.Id,
            GuestId = loadedReservation.GuestId,
            GuestName = $"{loadedReservation.Guest.FirstName} {loadedReservation.Guest.LastName}",
            RoomId = loadedReservation.RoomId,
            RoomNumber = loadedReservation.Room.RoomNumber,
            CheckInDate = loadedReservation.CheckInDate,
            CheckOutDate = loadedReservation.CheckOutDate,
            Status = loadedReservation.Status,
            CreatedAt = loadedReservation.CreatedAt
        };
    }

    public async Task<ReservationDto?> UpdateAsync(int id, CreateReservationDto createReservationDto)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            return null;
        }

        // Validate check-out is after check-in
        if (createReservationDto.CheckOutDate <= createReservationDto.CheckInDate)
        {
            return null;
        }

        reservation.CheckInDate = createReservationDto.CheckInDate;
        reservation.CheckOutDate = createReservationDto.CheckOutDate;

        await _reservationRepository.UpdateAsync(reservation);
        var updatedReservation = await _reservationRepository.GetByIdAsync(id);

        return new ReservationDto
        {
            Id = updatedReservation!.Id,
            GuestId = updatedReservation.GuestId,
            GuestName = $"{updatedReservation.Guest.FirstName} {updatedReservation.Guest.LastName}",
            RoomId = updatedReservation.RoomId,
            RoomNumber = updatedReservation.Room.RoomNumber,
            CheckInDate = updatedReservation.CheckInDate,
            CheckOutDate = updatedReservation.CheckOutDate,
            Status = updatedReservation.Status,
            CreatedAt = updatedReservation.CreatedAt
        };
    }

    public async Task<bool> CancelAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            return false;
        }

        // Update reservation status
        reservation.Status = ReservationStatus.Cancelled;
        await _reservationRepository.UpdateAsync(reservation);

        // Mark room as available
        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room != null)
        {
            room.IsAvailable = true;
            await _roomRepository.UpdateAsync(room);
        }

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _reservationRepository.DeleteAsync(id);
    }
}

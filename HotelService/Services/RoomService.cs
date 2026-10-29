using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;

namespace HotelService.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository)
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            RoomNumber = r.RoomNumber,
            RoomTypeId = r.RoomTypeId,
            RoomTypeName = r.RoomType?.Name ?? string.Empty,
            Rate = r.Rate,
            IsAvailable = r.IsAvailable
        });
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            return null;
        }

        return new RoomDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            RoomTypeId = room.RoomTypeId,
            RoomTypeName = room.RoomType?.Name ?? string.Empty,
            Rate = room.Rate,
            IsAvailable = room.IsAvailable
        };
    }

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync()
    {
        var rooms = await _roomRepository.GetAvailableRoomsAsync();
        return rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            RoomNumber = r.RoomNumber,
            RoomTypeId = r.RoomTypeId,
            RoomTypeName = r.RoomType?.Name ?? string.Empty,
            Rate = r.Rate,
            IsAvailable = r.IsAvailable
        });
    }

    public async Task<RoomDto?> CreateAsync(CreateRoomDto createRoomDto)
    {
        // Check if room type exists
        var roomType = await _roomTypeRepository.GetByIdAsync(createRoomDto.RoomTypeId);
        if (roomType == null)
        {
            return null;
        }

        var room = new Room
        {
            RoomNumber = createRoomDto.RoomNumber,
            RoomTypeId = createRoomDto.RoomTypeId,
            Rate = createRoomDto.Rate,
            IsAvailable = true
        };

        var createdRoom = await _roomRepository.CreateAsync(room);
        var loadedRoom = await _roomRepository.GetByIdAsync(createdRoom.Id);

        return new RoomDto
        {
            Id = loadedRoom!.Id,
            RoomNumber = loadedRoom.RoomNumber,
            RoomTypeId = loadedRoom.RoomTypeId,
            RoomTypeName = loadedRoom.RoomType?.Name ?? string.Empty,
            Rate = loadedRoom.Rate,
            IsAvailable = loadedRoom.IsAvailable
        };
    }

    public async Task<RoomDto?> UpdateAsync(int id, CreateRoomDto createRoomDto)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            return null;
        }

        // Check if room type exists
        var roomType = await _roomTypeRepository.GetByIdAsync(createRoomDto.RoomTypeId);
        if (roomType == null)
        {
            return null;
        }

        room.RoomNumber = createRoomDto.RoomNumber;
        room.RoomTypeId = createRoomDto.RoomTypeId;
        room.Rate = createRoomDto.Rate;

        await _roomRepository.UpdateAsync(room);
        var updatedRoom = await _roomRepository.GetByIdAsync(id);

        return new RoomDto
        {
            Id = updatedRoom!.Id,
            RoomNumber = updatedRoom.RoomNumber,
            RoomTypeId = updatedRoom.RoomTypeId,
            RoomTypeName = updatedRoom.RoomType?.Name ?? string.Empty,
            Rate = updatedRoom.Rate,
            IsAvailable = updatedRoom.IsAvailable
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _roomRepository.DeleteAsync(id);
    }
}

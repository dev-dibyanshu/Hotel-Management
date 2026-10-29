using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;

namespace HotelService.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<IEnumerable<RoomTypeDto>> GetAllAsync()
    {
        var roomTypes = await _roomTypeRepository.GetAllAsync();
        return roomTypes.Select(rt => new RoomTypeDto
        {
            Id = rt.Id,
            Name = rt.Name,
            Description = rt.Description,
            Capacity = rt.Capacity
        });
    }

    public async Task<RoomTypeDto?> GetByIdAsync(int id)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(id);
        if (roomType == null)
        {
            return null;
        }

        return new RoomTypeDto
        {
            Id = roomType.Id,
            Name = roomType.Name,
            Description = roomType.Description,
            Capacity = roomType.Capacity
        };
    }

    public async Task<RoomTypeDto> CreateAsync(CreateRoomTypeDto createRoomTypeDto)
    {
        var roomType = new RoomType
        {
            Name = createRoomTypeDto.Name,
            Description = createRoomTypeDto.Description,
            Capacity = createRoomTypeDto.Capacity
        };

        var createdRoomType = await _roomTypeRepository.CreateAsync(roomType);

        return new RoomTypeDto
        {
            Id = createdRoomType.Id,
            Name = createdRoomType.Name,
            Description = createdRoomType.Description,
            Capacity = createdRoomType.Capacity
        };
    }

    public async Task<RoomTypeDto?> UpdateAsync(int id, CreateRoomTypeDto createRoomTypeDto)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(id);
        if (roomType == null)
        {
            return null;
        }

        roomType.Name = createRoomTypeDto.Name;
        roomType.Description = createRoomTypeDto.Description;
        roomType.Capacity = createRoomTypeDto.Capacity;

        var updatedRoomType = await _roomTypeRepository.UpdateAsync(roomType);

        return new RoomTypeDto
        {
            Id = updatedRoomType!.Id,
            Name = updatedRoomType.Name,
            Description = updatedRoomType.Description,
            Capacity = updatedRoomType.Capacity
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _roomTypeRepository.DeleteAsync(id);
    }
}

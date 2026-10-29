using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Models;

namespace HotelService.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly AppDbContext _context;

    public RoomTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomType>> GetAllAsync()
    {
        return await _context.RoomTypes.ToListAsync();
    }

    public async Task<RoomType?> GetByIdAsync(int id)
    {
        return await _context.RoomTypes.FindAsync(id);
    }

    public async Task<RoomType> CreateAsync(RoomType roomType)
    {
        _context.RoomTypes.Add(roomType);
        await _context.SaveChangesAsync();
        return roomType;
    }

    public async Task<RoomType?> UpdateAsync(RoomType roomType)
    {
        _context.RoomTypes.Update(roomType);
        await _context.SaveChangesAsync();
        return roomType;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var roomType = await _context.RoomTypes.FindAsync(id);
        if (roomType == null)
        {
            return false;
        }

        _context.RoomTypes.Remove(roomType);
        await _context.SaveChangesAsync();
        return true;
    }
}

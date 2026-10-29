using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Models;

namespace HotelService.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms.Include(r => r.RoomType).ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _context.Rooms.Include(r => r.RoomType).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
    {
        return await _context.Rooms
            .Include(r => r.RoomType)
            .Where(r => r.IsAvailable)
            .ToListAsync();
    }

    public async Task<Room> CreateAsync(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<Room?> UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return false;
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }
}

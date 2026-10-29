using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Models;

namespace HotelService.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly AppDbContext _context;

    public GuestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Guest>> GetAllAsync()
    {
        return await _context.Guests.ToListAsync();
    }

    public async Task<Guest?> GetByIdAsync(int id)
    {
        return await _context.Guests.FindAsync(id);
    }

    public async Task<Guest> CreateAsync(Guest guest)
    {
        _context.Guests.Add(guest);
        await _context.SaveChangesAsync();
        return guest;
    }

    public async Task<Guest?> UpdateAsync(Guest guest)
    {
        _context.Guests.Update(guest);
        await _context.SaveChangesAsync();
        return guest;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var guest = await _context.Guests.FindAsync(id);
        if (guest == null)
        {
            return false;
        }

        _context.Guests.Remove(guest);
        await _context.SaveChangesAsync();
        return true;
    }
}

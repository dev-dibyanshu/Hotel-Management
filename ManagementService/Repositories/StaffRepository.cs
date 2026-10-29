using Microsoft.EntityFrameworkCore;
using ManagementService.Data;
using ManagementService.Models;

namespace ManagementService.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Staff>> GetAllAsync()
    {
        return await _context.Staff.Include(s => s.Department).ToListAsync();
    }

    public async Task<Staff?> GetByIdAsync(int id)
    {
        return await _context.Staff.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Staff> CreateAsync(Staff staff)
    {
        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();
        return staff;
    }

    public async Task<Staff?> UpdateAsync(Staff staff)
    {
        _context.Staff.Update(staff);
        await _context.SaveChangesAsync();
        return staff;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var staff = await _context.Staff.FindAsync(id);
        if (staff == null)
        {
            return false;
        }

        _context.Staff.Remove(staff);
        await _context.SaveChangesAsync();
        return true;
    }
}

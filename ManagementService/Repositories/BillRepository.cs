using Microsoft.EntityFrameworkCore;
using ManagementService.Data;
using ManagementService.Models;

namespace ManagementService.Repositories;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _context;

    public BillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync()
    {
        return await _context.Bills.ToListAsync();
    }

    public async Task<Bill?> GetByIdAsync(int id)
    {
        return await _context.Bills.FindAsync(id);
    }

    public async Task<Bill> CreateAsync(Bill bill)
    {
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();
        return bill;
    }

    public async Task<Bill?> UpdateAsync(Bill bill)
    {
        _context.Bills.Update(bill);
        await _context.SaveChangesAsync();
        return bill;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill == null)
        {
            return false;
        }

        _context.Bills.Remove(bill);
        await _context.SaveChangesAsync();
        return true;
    }
}

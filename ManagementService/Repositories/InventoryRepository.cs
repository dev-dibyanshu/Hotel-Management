using Microsoft.EntityFrameworkCore;
using ManagementService.Data;
using ManagementService.Models;

namespace ManagementService.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;

    public InventoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync()
    {
        return await _context.InventoryItems.ToListAsync();
    }

    public async Task<InventoryItem?> GetByIdAsync(int id)
    {
        return await _context.InventoryItems.FindAsync(id);
    }

    public async Task<InventoryItem> CreateAsync(InventoryItem inventoryItem)
    {
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task<InventoryItem?> UpdateAsync(InventoryItem inventoryItem)
    {
        _context.InventoryItems.Update(inventoryItem);
        await _context.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var inventoryItem = await _context.InventoryItems.FindAsync(id);
        if (inventoryItem == null)
        {
            return false;
        }

        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();
        return true;
    }
}

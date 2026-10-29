using ManagementService.Models;

namespace ManagementService.Repositories;

public interface IInventoryRepository
{
    Task<IEnumerable<InventoryItem>> GetAllAsync();
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<InventoryItem> CreateAsync(InventoryItem inventoryItem);
    Task<InventoryItem?> UpdateAsync(InventoryItem inventoryItem);
    Task<bool> DeleteAsync(int id);
}

using ManagementService.DTOs;

namespace ManagementService.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryItemDto>> GetAllAsync();
    Task<InventoryItemDto?> GetByIdAsync(int id);
    Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto createInventoryItemDto);
    Task<InventoryItemDto?> UpdateAsync(int id, CreateInventoryItemDto createInventoryItemDto);
    Task<InventoryItemDto?> UpdateQuantityAsync(int id, int quantity);
    Task<bool> DeleteAsync(int id);
}

using ManagementService.DTOs;
using ManagementService.Models;
using ManagementService.Repositories;

namespace ManagementService.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<IEnumerable<InventoryItemDto>> GetAllAsync()
    {
        var items = await _inventoryRepository.GetAllAsync();
        return items.Select(i => new InventoryItemDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            CreatedAt = i.CreatedAt
        });
    }

    public async Task<InventoryItemDto?> GetByIdAsync(int id)
    {
        var item = await _inventoryRepository.GetByIdAsync(id);
        if (item == null)
        {
            return null;
        }

        return new InventoryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            CreatedAt = item.CreatedAt
        };
    }

    public async Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto createInventoryItemDto)
    {
        var item = new InventoryItem
        {
            Name = createInventoryItemDto.Name,
            Description = createInventoryItemDto.Description,
            Quantity = createInventoryItemDto.Quantity,
            UnitPrice = createInventoryItemDto.UnitPrice,
            CreatedAt = DateTime.UtcNow
        };

        var createdItem = await _inventoryRepository.CreateAsync(item);

        return new InventoryItemDto
        {
            Id = createdItem.Id,
            Name = createdItem.Name,
            Description = createdItem.Description,
            Quantity = createdItem.Quantity,
            UnitPrice = createdItem.UnitPrice,
            CreatedAt = createdItem.CreatedAt
        };
    }

    public async Task<InventoryItemDto?> UpdateAsync(int id, CreateInventoryItemDto createInventoryItemDto)
    {
        var item = await _inventoryRepository.GetByIdAsync(id);
        if (item == null)
        {
            return null;
        }

        item.Name = createInventoryItemDto.Name;
        item.Description = createInventoryItemDto.Description;
        item.Quantity = createInventoryItemDto.Quantity;
        item.UnitPrice = createInventoryItemDto.UnitPrice;

        var updatedItem = await _inventoryRepository.UpdateAsync(item);

        return new InventoryItemDto
        {
            Id = updatedItem!.Id,
            Name = updatedItem.Name,
            Description = updatedItem.Description,
            Quantity = updatedItem.Quantity,
            UnitPrice = updatedItem.UnitPrice,
            CreatedAt = updatedItem.CreatedAt
        };
    }

    public async Task<InventoryItemDto?> UpdateQuantityAsync(int id, int quantity)
    {
        var item = await _inventoryRepository.GetByIdAsync(id);
        if (item == null)
        {
            return null;
        }

        item.Quantity = quantity;
        var updatedItem = await _inventoryRepository.UpdateAsync(item);

        return new InventoryItemDto
        {
            Id = updatedItem!.Id,
            Name = updatedItem.Name,
            Description = updatedItem.Description,
            Quantity = updatedItem.Quantity,
            UnitPrice = updatedItem.UnitPrice,
            CreatedAt = updatedItem.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _inventoryRepository.DeleteAsync(id);
    }
}

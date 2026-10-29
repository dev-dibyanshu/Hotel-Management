using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ManagementService.DTOs;
using ManagementService.Services;

namespace ManagementService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _inventoryService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _inventoryService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(new { message = "Inventory item not found" });
        }

        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Staff")]
    public async Task<IActionResult> Create([FromBody] CreateInventoryItemDto createInventoryItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var item = await _inventoryService.CreateAsync(createInventoryItemDto);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateInventoryItemDto createInventoryItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var item = await _inventoryService.UpdateAsync(id, createInventoryItemDto);
        if (item == null)
        {
            return NotFound(new { message = "Inventory item not found" });
        }

        return Ok(item);
    }

    [HttpPut("{id}/quantity")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    public async Task<IActionResult> UpdateQuantity(int id, [FromBody] UpdateInventoryQuantityDto updateQuantityDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var item = await _inventoryService.UpdateQuantityAsync(id, updateQuantityDto.Quantity);
        if (item == null)
        {
            return NotFound(new { message = "Inventory item not found" });
        }

        return Ok(item);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _inventoryService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Inventory item not found" });
        }

        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelService.DTOs;
using HotelService.Services;

namespace HotelService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypesController(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roomTypes = await _roomTypeService.GetAllAsync();
        return Ok(roomTypes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var roomType = await _roomTypeService.GetByIdAsync(id);
        if (roomType == null)
        {
            return NotFound(new { message = "Room type not found" });
        }

        return Ok(roomType);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomTypeDto createRoomTypeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var roomType = await _roomTypeService.CreateAsync(createRoomTypeDto);
        return CreatedAtAction(nameof(GetById), new { id = roomType.Id }, roomType);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateRoomTypeDto createRoomTypeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var roomType = await _roomTypeService.UpdateAsync(id, createRoomTypeDto);
        if (roomType == null)
        {
            return NotFound(new { message = "Room type not found" });
        }

        return Ok(roomType);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _roomTypeService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Room type not found" });
        }

        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelService.DTOs;
using HotelService.Services;

namespace HotelService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();
        return Ok(rooms);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var rooms = await _roomService.GetAvailableRoomsAsync();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null)
        {
            return NotFound(new { message = "Room not found" });
        }

        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto createRoomDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var room = await _roomService.CreateAsync(createRoomDto);
        if (room == null)
        {
            return BadRequest(new { message = "Room type not found" });
        }

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateRoomDto createRoomDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var room = await _roomService.UpdateAsync(id, createRoomDto);
        if (room == null)
        {
            return NotFound(new { message = "Room or room type not found" });
        }

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _roomService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Room not found" });
        }

        return NoContent();
    }
}

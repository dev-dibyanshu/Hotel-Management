using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelService.DTOs;
using HotelService.Services;

namespace HotelService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;

    public GuestsController(IGuestService guestService)
    {
        _guestService = guestService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var guests = await _guestService.GetAllAsync();
        return Ok(guests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var guest = await _guestService.GetByIdAsync(id);
        if (guest == null)
        {
            return NotFound(new { message = "Guest not found" });
        }

        return Ok(guest);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestDto createGuestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var guest = await _guestService.CreateAsync(createGuestDto);
        return CreatedAtAction(nameof(GetById), new { id = guest.Id }, guest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateGuestDto createGuestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var guest = await _guestService.UpdateAsync(id, createGuestDto);
        if (guest == null)
        {
            return NotFound(new { message = "Guest not found" });
        }

        return Ok(guest);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _guestService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Guest not found" });
        }

        return NoContent();
    }
}

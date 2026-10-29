using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelService.DTOs;
using HotelService.Services;

namespace HotelService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reservations = await _reservationService.GetAllAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);
        if (reservation == null)
        {
            return NotFound(new { message = "Reservation not found" });
        }

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto createReservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var reservation = await _reservationService.CreateAsync(createReservationDto);
        if (reservation == null)
        {
            return BadRequest(new { message = "Invalid reservation. Check guest exists, room exists, room is available, and checkout is after checkin." });
        }

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateReservationDto createReservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var reservation = await _reservationService.UpdateAsync(id, createReservationDto);
        if (reservation == null)
        {
            return NotFound(new { message = "Reservation not found or invalid dates" });
        }

        return Ok(reservation);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _reservationService.CancelAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Reservation not found" });
        }

        return Ok(new { message = "Reservation cancelled successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reservationService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Reservation not found" });
        }

        return NoContent();
    }
}

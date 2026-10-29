using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ManagementService.DTOs;
using ManagementService.Services;

namespace ManagementService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var staff = await _staffService.GetAllAsync();
        return Ok(staff);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null)
        {
            return NotFound(new { message = "Staff member not found" });
        }

        return Ok(staff);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] CreateStaffDto createStaffDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var staff = await _staffService.CreateAsync(createStaffDto);
        if (staff == null)
        {
            return BadRequest(new { message = "Department not found" });
        }

        return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateStaffDto createStaffDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var staff = await _staffService.UpdateAsync(id, createStaffDto);
        if (staff == null)
        {
            return NotFound(new { message = "Staff member or department not found" });
        }

        return Ok(staff);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _staffService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Staff member not found" });
        }

        return NoContent();
    }
}

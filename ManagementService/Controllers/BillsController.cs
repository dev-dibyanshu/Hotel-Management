using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ManagementService.DTOs;
using ManagementService.Services;

namespace ManagementService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BillsController : ControllerBase
{
    private readonly IBillService _billService;

    public BillsController(IBillService billService)
    {
        _billService = billService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bills = await _billService.GetAllAsync();
        return Ok(bills);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var bill = await _billService.GetByIdAsync(id);
        if (bill == null)
        {
            return NotFound(new { message = "Bill not found" });
        }

        return Ok(bill);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Staff")]
    public async Task<IActionResult> Create([FromBody] CreateBillDto createBillDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var bill = await _billService.CreateAsync(createBillDto);
        return CreatedAtAction(nameof(GetById), new { id = bill.Id }, bill);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateBillDto createBillDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var bill = await _billService.UpdateAsync(id, createBillDto);
        if (bill == null)
        {
            return NotFound(new { message = "Bill not found" });
        }

        return Ok(bill);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _billService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Bill not found" });
        }

        return NoContent();
    }
}

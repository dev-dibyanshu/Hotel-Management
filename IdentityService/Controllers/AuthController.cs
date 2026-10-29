using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityService.DTOs;
using IdentityService.Services;
using System.Security.Claims;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (result == null)
        {
            return Conflict(new { message = "Email already exists" });
        }

        return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value 
                    ?? User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            userId,
            email,
            role
        });
    }
}

public class JwtRegisteredClaimNames
{
    public const string Sub = "sub";
    public const string Email = "email";
}

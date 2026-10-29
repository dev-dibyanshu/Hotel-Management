using IdentityService.DTOs;

namespace IdentityService.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
}

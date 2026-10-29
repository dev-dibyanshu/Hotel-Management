using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using IdentityService.DTOs;
using IdentityService.Models;
using IdentityService.Repositories;

namespace IdentityService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        // Check if email already exists
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return null;
        }

        // Create new user
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Role = UserRole.Guest,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

        // Save user
        await _userRepository.CreateAsync(user);

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return null;
        }

        // Verify password
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
        var jwtAudience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
        var jwtExpiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

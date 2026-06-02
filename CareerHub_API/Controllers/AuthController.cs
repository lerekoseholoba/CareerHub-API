using CareerHub_API.DTOs;
using CareerHub_API.Models;
using CareerHub_API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerHub_API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Username != "employer" ||
            request.Password != "password123")
        {
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,
                request.Username),

            new Claim(ClaimTypes.Role,
                "Employer")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        var tokenString =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return Ok(new LoginResponse(tokenString));
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var username =
            User.FindFirstValue(
                JwtRegisteredClaimNames.Sub
            );

        var role =
            User.FindFirstValue(
                ClaimTypes.Role
            );

        return Ok(new
        {
            Username = username,
            Role = role
        });
    }
}
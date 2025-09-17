using Microsoft.AspNetCore.Mvc;
using EventReg.Application.Interfaces;
using EventReg.Application.DTOs;

namespace EventReg.Api.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var authResponse = await _authService.LoginAsync(dto);
        if (authResponse == null) return Unauthorized(new { message = "Invalid username or password" });
        
        return Ok(authResponse);
    }
}

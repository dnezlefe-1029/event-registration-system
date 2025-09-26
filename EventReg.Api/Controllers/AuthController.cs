using Microsoft.AspNetCore.Mvc;
using EventReg.Application.Interfaces;
using EventReg.Application.DTOs;

namespace EventReg.Api.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        _logger.LogInformation("Login attempt for ${Email}", dto.Email);

        var authResponse = await _authService.LoginAsync(dto);
        if (authResponse == null)
        {
            _logger.LogError("Login failed for ${Email}", dto.Email);
            return Unauthorized("Invalid email or password");
        }
        
        return Ok(authResponse);
    }
}

using EventReg.Application.DTOs;

namespace EventReg.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}

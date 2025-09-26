using EventReg.Application.DTOs;

namespace EventReg.Application.Interfaces;

public interface IRegistrationService
{
    Task<PagedResult<RegistrationDto>> GetAllAsync(RegistrationQueryParameters parameters);
    Task<RegistrationDto?> GetByIdAsync(int id);
    Task<RegistrationDto> CreateAsync(RegistrationCreateDto dto);
    Task<RegistrationDto?> UpdateAsync(int id, RegistrationUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
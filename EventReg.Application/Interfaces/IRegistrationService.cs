using EventReg.Domain.Entities;
using EventReg.Application.DTOs;

namespace EventReg.Application.Interfaces;

public interface IRegistrationService
{
    Task<IEnumerable<RegistrationDto>> GetAllAsync();
    Task<RegistrationDto?> GetByIdAsync(int id);
    Task<RegistrationDto> CreateAsync(RegistrationCreateDto dto);
    Task<RegistrationDto?> UpdateAsync(int id, RegistrationUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

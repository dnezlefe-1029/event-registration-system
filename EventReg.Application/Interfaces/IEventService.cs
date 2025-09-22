using EventReg.Domain.Entities;
using EventReg.Application.DTOs;

namespace EventReg.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventDto>> GellAllAsync();
    Task<EventDto?> GetByIdAsync(int id);
    Task<EventDto> CreateAsync(EventCreateDto ev);
    Task<EventDto?> UpdateAsync(int id, EventCreateDto ev);
    Task<bool> DeleteAsync(int id);
}

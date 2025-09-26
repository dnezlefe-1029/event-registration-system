using EventReg.Application.DTOs;

namespace EventReg.Application.Interfaces;

public interface IEventService
{
    Task<PagedResult<EventDto>> GetAllAsync(EventQueryParameters eventQuery);
    Task<EventDto?> GetByIdAsync(int id);
    Task<EventDto> CreateAsync(EventCreateDto ev);
    Task<EventDto?> UpdateAsync(int id, EventCreateDto ev);
    Task<bool> DeleteAsync(int id);
}

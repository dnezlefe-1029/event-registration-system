using EventReg.Application.Interfaces;
using EventReg.Domain.Entities;
using EventReg.Persistence;
using Microsoft.EntityFrameworkCore;
using EventReg.Application.DTOs;
using AutoMapper;
using EventReg.Application.Extentions;

namespace EventReg.Application.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedResult<EventDto>> GetAllAsync(EventQueryParameters eventQuery)
    {
        var query = _dbContext.Events.AsQueryable();

        if (!string.IsNullOrEmpty(eventQuery.Search))
        {
            query = query.Where(e => e.Title.Contains(eventQuery.Search));
        }

        if (eventQuery.StartDate.HasValue && eventQuery.EndDate.HasValue)
        {
            if (eventQuery.StartDate.Value > eventQuery.EndDate.Value)
            {
                throw new ArgumentException("StartDate cannot be greater that EndDate");
            }

            query = query.Where(e => e.StartDate >= eventQuery.StartDate.Value && 
                e.EndDate <= eventQuery.EndDate.Value);
        }

        return await query.OrderBy(e => e.StartDate)
            .Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
            }).ToPagedResultAsync(eventQuery.PageNumber, eventQuery.PageSize);
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        var ev = await _dbContext.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id);

        return _mapper.Map<EventDto?>(ev);
    }

    public async Task<EventDto> CreateAsync(EventCreateDto dto)
    {
        var ev = _mapper.Map<Event>(dto);
        _dbContext.Events.Add(ev);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<EventDto>(ev);
    }

    public async Task<EventDto?> UpdateAsync(int id, EventCreateDto dto)
    {
        var existEv = await _dbContext.Events.FindAsync(id);
        if (existEv == null) return null;

        _mapper.Map(dto, existEv);

        _dbContext.Events.Update(existEv);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<EventDto>(existEv);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ev = await _dbContext.Events.FindAsync(id);
        if (ev == null) return false;

        _dbContext.Events.Remove(ev);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

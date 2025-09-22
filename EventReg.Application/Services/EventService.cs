using EventReg.Application.Interfaces;
using EventReg.Domain.Entities;
using EventReg.Persistence;
using Microsoft.EntityFrameworkCore;
using EventReg.Application.DTOs;
using AutoMapper;

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

    public async Task<IEnumerable<EventDto>> GellAllAsync()
    {
        var result =  await _dbContext.Events
            .Include(e => e.Registrations)
            .ToListAsync();

        return _mapper.Map<IEnumerable<EventDto>>(result);
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        var ev = await _dbContext.Events.FindAsync(id);

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

using EventReg.Application.Interfaces;
using EventReg.Domain.Entities;
using EventReg.Persistence;
using Microsoft.EntityFrameworkCore;
using EventReg.Application.DTOs;
using AutoMapper;
using EventReg.Application.Extentions;

namespace EventReg.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public RegistrationService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedResult<RegistrationDto>> GetAllAsync(RegistrationQueryParameters parameters)
    {
        var query = _dbContext.Registrations.AsQueryable();

        if (parameters.EventId.HasValue)
        {
            query = query.Where(r => r.EventId == parameters.EventId);
        }

        if (!string.IsNullOrEmpty(parameters.Search))
        {
            query = query.Where(r =>
                r.AttendeeName.Contains(parameters.Search) ||
                r.AttendeeEmail.Contains(parameters.Search));
        }

        return await query
            .OrderByDescending(r => r.RegisteredAt)
            .Select(r => new RegistrationDto
            {
                Id = r.Id,
                EventId = r.EventId,
                UserId = r.User == null ? null : r.User.Id,
                AttendeeName = r.AttendeeName,
                AttendeeEmail = r.AttendeeEmail,
                RegistredAt = r.RegisteredAt,
                User = r.User == null ? null : new UserDto
                {
                    Id = r.User.Id,
                    Name = r.User.Name,
                    Email = r.User.Email
                }
            }).ToPagedResultAsync(parameters.PageNumber, parameters.PageSize);
    }

    public async Task<RegistrationDto?> GetByIdAsync(int eventId)
    {
        var result = await _dbContext.Registrations
            .FirstOrDefaultAsync(r => r.Id == eventId);

        return _mapper?.Map<RegistrationDto?>(result);
    }

    public async Task<RegistrationDto> CreateAsync(RegistrationCreateDto dto)
    {
        var registration = _mapper.Map<Registration>(dto);
        _dbContext.Registrations.Add(registration);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<RegistrationDto>(registration);
    }

    public async Task<RegistrationDto?> UpdateAsync(int id, RegistrationUpdateDto dto)
    {
        var existingRegistration = await _dbContext.Registrations.FindAsync(id);
        if (existingRegistration == null) return null;

        _mapper.Map(dto, existingRegistration);

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<RegistrationDto>(existingRegistration);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var registration = await _dbContext.Registrations.FindAsync(id);
        if (registration == null) return false;

        _dbContext.Registrations.Remove(registration);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventReg.Domain.Entities;
using EventReg.Application.Interfaces;
using EventReg.Application.Services;
using EventReg.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EventReg.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventService.GellAllAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _eventService.GetByIdAsync(id);
        if (ev == null) return NotFound();
        return Ok(ev);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EventCreateDto ev)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdEvent = await _eventService.CreateAsync(ev);
        return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id }, createdEvent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EventUpdateDto ev)
    {
        var updated = await _eventService.UpdateAsync(id, ev);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _eventService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}

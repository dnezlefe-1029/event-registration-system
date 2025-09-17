using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventReg.Domain.Entities;
using EventReg.Application.DTOs;
using EventReg.Application.Interfaces;

namespace EventReg.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _registrationService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reg = await _registrationService.GetByIdAsync(id);
        if (reg == null) return NotFound();
        return Ok(reg);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegistrationCreateDto registration)
    {
        try
        {
            var reg = await _registrationService.CreateAsync(registration);
            return CreatedAtAction(nameof(GetById), new { Id = reg.Id }, reg);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RegistrationUpdateDto registration)
    {
        var updatedReg = await _registrationService.UpdateAsync(id, registration);
        if (updatedReg == null) return NotFound();
        return Ok(updatedReg);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _registrationService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}

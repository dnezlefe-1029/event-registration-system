using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventReg.Application.Interfaces;
using EventReg.Application.DTOs;

namespace EventReg.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto createUserDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdUser = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto updateUserDto)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
        if (updatedUser == null) return NotFound();
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}

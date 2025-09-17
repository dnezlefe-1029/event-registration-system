using EventReg.Domain.Entities;
using EventReg.Application.Interfaces;
using EventReg.Application.DTOs;
using EventReg.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EventReg.Application.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(ApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Username = user.Username
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _dbContext.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Username = user.Username
            })
            .ToListAsync();
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto createUserDto)
    {
        var user = new User
        {
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            Username = createUserDto.Username,
            Role = createUserDto.Role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, createUserDto.Password);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Username = user.Username
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UserUpdateDto updateUserDto)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return null;

        user.Name = updateUserDto.Name;
        user.Role = updateUserDto.Role;

        await _dbContext.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            Email = user.Email,
            Username = user.Username
        };
    }
}

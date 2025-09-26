using EventReg.Application.DTOs;

namespace EventReg.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(UserCreateDto createUserDto);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<PagedResult<UserDto>> GetAllUsersAsync(QueryParameters queryParameters);
    Task<UserDto> UpdateUserAsync(int id, UserUpdateDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
}

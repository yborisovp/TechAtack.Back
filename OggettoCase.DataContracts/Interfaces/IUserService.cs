using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Interfaces;

public interface IUserService : IBaseService<UserDto, long, UpdateUserDto>
{
    public Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct = default);
    public Task<UserDto> CreateUserAsync(CreateUserDto createUserParams, CancellationToken ct);
}
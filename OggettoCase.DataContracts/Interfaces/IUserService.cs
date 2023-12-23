using System.Collections;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Filters;

namespace OggettoCase.DataContracts.Interfaces;

public interface IUserService : IBaseService<UserDto, long, UpdateUserDto>
{
    public Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct = default);
    public Task<UserDto> CreateUserAsync(CreateUserDto createUserParams, string externalAccessToken = "", CancellationToken ct = default);
    public Task<AuthorizationInfoDto> AuthorizeUserAsync(long userId, string externalAccessToken = "", CancellationToken ct = default);
    Task ApproveUserAccountAsync(long userId, CancellationToken ct = default);
    Task<IEnumerable<UserDto>> GetByFilterAsync(UserFilter userFilter, CancellationToken ct = default);
}
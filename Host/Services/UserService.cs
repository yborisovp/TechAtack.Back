using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Dtos.Users.Enums;
using OggettoCase.DataContracts.Filters;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using OggettoCase.Mappers.Comments;
using OggettoCase.Mappers.Filters;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Services;

/// <summary>
/// Service to grant access to db entities
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly ITokenGenerator _tokenGenerator;
    
    /// <summary>
    /// Constructor of an service
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="logger"></param>
    public UserService(IUserRepository userRepository, ILogger<UserService> logger, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _tokenGenerator = tokenGenerator;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(UserDto));

        var users = await _userRepository.GetAllAsync(ct);

        _logger.LogDebug("Successfully received list of {name of}", nameof(UserDto));
        return users.Select(DbToDtoUserMapper.ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<UserDto> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} with id: '{id}'", nameof(UserDto), id);
        var user = await _userRepository.GetByIdAsync(id, ct);

        if (user is null)
        {
            throw new KeyNotFoundException($"{nameof(UserDto)} with id: '{id}' doesn't exist");
        }

        _logger.LogDebug("Successfully received {name of} with id: '{id}'", nameof(UserDto), id);
        return user.ToDto();
    }

    /// <inheritdoc />
    public async Task<UserDto> UpdateByIdAsync(long id, UpdateUserDto dtoToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}'", nameof(UserDto), id);
        var user = await _userRepository.GetByIdAsync(id, ct);

        if (user is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while update", nameof(UserDto), id);
            throw new KeyNotFoundException($"{nameof(UserDto)} with id: '{id}' doesn't exist");
        }

        var templateToUpdate = dtoToUpdate.ToEntity(id);

        var updatedTemplate = await _userRepository.UpdateAsync(templateToUpdate, ct);
        if (updatedTemplate is null)
        {
            _logger.LogError("Cannot update {name of} with id: '{id}'", nameof(UserDto), id);
            throw new InvalidProgramException($"Cannot update {nameof(UserDto)} with id: '{id}'");
        }

        var result = await _userRepository.GetByIdAsync(id, ct);

        _logger.LogDebug("Successfully updated {name of} with id: '{id}'", nameof(UserDto), id);
        return result.ToDto();
    }

    /// <inheritdoc />
    public async Task<long> DeleteByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}'", nameof(UserDto), id);
        var user = await _userRepository.GetByIdAsync(id, ct);
        if (user is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while deleting", nameof(UserDto), id);
            throw new KeyNotFoundException($"{nameof(UserDto)} with id: '{id}' cannot be deleted");
        }

        var deletedId = await _userRepository.DeleteByIdAsync(user.Id, ct);

        _logger.LogDebug("Successfully delete {name of} with id: '{id}'", nameof(UserDto), id);
        return deletedId;
    }
    
    /// <inheritdoc />
    public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} with email: '{id}'", nameof(UserDto), email);
        var user = await _userRepository.GetByEmailAsync(email, ct);

        if (user is null)
        {
            _logger.LogDebug("{nameof(UserDto)} with email: '{email}' doesn't exist", nameof(UserDto), email);
            return null;
        }

        _logger.LogDebug("Successfully received {name of} with id: '{id}'", nameof(UserDto), email);

        return user.ToDto();
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserParams, string externalAccessToken = "", CancellationToken ct = default)
    {
        var user = await _userRepository.CreateUserAsync(createUserParams.ToEntity(), ct);
        
        return user.ToDto();
    }

    public async Task<AuthorizationInfoDto> AuthorizeUserAsync(long userId, string externalAccessToken = "", CancellationToken ct = default)
    {
        var user =await _userRepository.GetByIdAsync(userId, ct);
        var jwt = _tokenGenerator.GenerateJwt(user, externalAccessToken);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();
        await AssignRefreshToken(refreshToken, user, ct);
        
       return new ()
        {
            AccessToken = jwt,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task ApproveUserAccountAsync(long userId, UserRoleEnumDto approvedRole, CancellationToken ct = default)
    {
        _logger.LogDebug("Approve {name of} with id: '{id}'", nameof(UserDto), userId);
         await _userRepository.ApproveUserAccountAsync(userId, approvedRole.ToEntity(), ct);
    }

    public async Task<IEnumerable<UserDto>> GetByFilterAsync(UserFilter userFilter, CancellationToken ct = default)
    {
        var users = await _userRepository.GetByFilterAsync(userFilter.ToInternalFilter(), ct);
        return users.Select(x => x.ToDto()); 
    }

    private async Task AssignRefreshToken(RefreshToken refreshToken, User user, CancellationToken ct = default)
    {
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpirationDate = refreshToken.TokenExpirationDate;

        await _userRepository.UpdateAsync(user, ct);
    }
}
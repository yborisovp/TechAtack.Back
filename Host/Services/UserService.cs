using ServiceTemplate.DataAccess.Interfaces;
using ServiceTemplate.DataContracts.Dtos.Users;
using ServiceTemplate.DataContracts.Interfaces;
using ServiceTemplate.Mappers.Templates;
using ServiceTemplate.Mappers.Users;

namespace ServiceTemplate.Services;

/// <summary>
/// Service to grant access to db entities
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<TemplateService> _logger;

    /// <summary>
    /// Constructor of an service
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="logger"></param>
    public UserService(IUserRepository userRepository, ILogger<TemplateService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
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
}
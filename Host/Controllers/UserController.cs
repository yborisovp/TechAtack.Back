using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace OggettoCase.Controllers;

/// <summary>
/// API to control templates
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class UserController : ControllerBase, IUserController
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    /// <summary>
    /// Constructor of TemplateController
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="logger"></param>
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <inheritdoc />
    [HttpGet]
    [SwaggerOperation($"Get all {nameof(UserDto)}s")]
    [SwaggerResponse(200, type: typeof(IEnumerable<UserDto>), description: $"List of {nameof(UserDto)}s")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(UserDto));
        var UserDtos = await _userService.GetAllAsync(ct);
        _logger.LogDebug("Successfully received list of {UserDto}s", nameof(UserDto));
        return Ok(UserDtos);
    }

    /// <inheritdoc />
    [HttpGet("{id:guid}")]
    [SwaggerOperation($"Get one {nameof(UserDto)}")]
    [SwaggerResponse(200, type: typeof(UserDto), description: $"Receive one {nameof(UserDto)} by id")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(UserDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<UserDto>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} with id: '{id}'", nameof(UserDto), id);
        var userDto = await _userService.GetByIdAsync(id, ct);
        _logger.LogDebug("Successfully received one {UserDto} by id: '{id}'", nameof(UserDto), id);
        return Ok(userDto);
    }

    /// <inheritdoc />
    [HttpPut("{id:Guid}")]
    [SwaggerOperation($"Update {nameof(UserDto)}")]
    [SwaggerResponse(200, type: typeof(UserDto), description: $"{nameof(UserDto)} successfully updated")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(UserDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<UserDto>> UpdateByIdAsync(long id, UpdateUserDto dtoToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}''", nameof(UserDto), id);
        var updatedTemplate = await _userService.UpdateByIdAsync(id, dtoToUpdate, ct);
        _logger.LogDebug("Successfully update {UserDto} by id: '{id}'", nameof(UserDto), id);

        return Ok(updatedTemplate);
    }

    /// <inheritdoc />
    [HttpDelete("{id:Guid}")]
    [SwaggerOperation($"Delete {nameof(UserDto)}")]
    [SwaggerResponse(200, type: typeof(UserDto), description: $"{nameof(UserDto)} successfully deleted")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(UserDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<long>> DeleteByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}'", nameof(UserDto), id);
        var deletedTemplateId = await _userService.DeleteByIdAsync(id, ct);
        _logger.LogDebug("Successfully delete {UserDto} by id: '{id}'", nameof(UserDto), id);

        return Ok(deletedTemplateId);
    }
}
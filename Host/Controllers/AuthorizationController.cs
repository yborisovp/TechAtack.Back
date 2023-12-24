using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataAccess.Models.Users.Enums;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Dtos.Users.Enums;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace OggettoCase.Controllers;

/// <summary>
/// Controller that work with authorization 
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class AuthorizationController: ControllerBase
{
    private readonly IGoogleService _googleService;
    private readonly IUserService _userService;
    private ITokenGenerator _tokenGenerator;
    
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationController(IGoogleService googleService, IUserService userService, ITokenGenerator tokenGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _googleService = googleService;
        _userService = userService;
        _tokenGenerator = tokenGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Authorize user 
    /// </summary>
    /// <param name="authorizeUserDto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost("authorize-user")]
    [SwaggerOperation($"Create {nameof(AuthorizationInfoDto)}")]
    [SwaggerResponse(200, type: typeof(AuthorizationInfoDto), description: $"{nameof(AuthorizationInfoDto)} successfully authorized")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<AuthorizationInfoDto> AuthorizeUser([FromBody] AuthorizeUserDto authorizeUserDto, CancellationToken ct)
    {
        var userAuthInfo = await _googleService.AuthorizeUserAsync(authorizeUserDto, ct);
        var user = await _userService.GetUserByEmailAsync(userAuthInfo.Email, ct);
        if (user is not null)
        {
            return await _userService.AuthorizeUserAsync(user.Id, authorizeUserDto.AccessToken, ct);
        }
        
        var createUserParams = new CreateUserDto(
            userAuthInfo.ExternalId,
            userAuthInfo.Email,
            userAuthInfo.Name,
            userAuthInfo.Surname,
            userAuthInfo.PictureUrl,
            UserAuthenticationTypeEnumDto.Google,
            authorizeUserDto.AccessToken
        );
        user = await _userService.CreateUserAsync(createUserParams, authorizeUserDto.AccessToken, ct);
        
        return await _userService.AuthorizeUserAsync(user.Id, authorizeUserDto.AccessToken, ct);
         
    }
    
    /// <summary>
    /// Get authorized user data
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [Authorize]
    [HttpGet("get-me")]
    [SwaggerOperation($"Create {nameof(AuthorizationInfoDto)}")]
    [SwaggerResponse(200, type: typeof(AuthorizationInfoDto), description: $"{nameof(AuthorizationInfoDto)} successfully authorized")]
    [SwaggerResponse(403, description: "Authorization error")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<UserDto> GetMeAsync(CancellationToken ct)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("userId") ?? throw new Exception();
        var user = await _userService.GetByIdAsync(long.Parse(userId), ct);
        return user;
    }
}


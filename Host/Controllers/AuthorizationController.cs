using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataAccess.Models.Users.Enums;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Dtos.Users.Enums;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;

namespace OggettoCase.Controllers;

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

    [HttpPost("authorize-user")]
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
    
    [Authorize]
    [HttpGet("get-me")]
    public async Task<UserDto> GetMeAsync(CancellationToken ct)
    {
        var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue("userId") ?? throw new Exception();
        var user = await _userService.GetByIdAsync(long.Parse(userId), ct);
        return user;
    }
}


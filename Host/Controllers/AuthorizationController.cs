using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataAccess.Models.Users.Enums;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Dtos.Users.Enums;
using OggettoCase.DataContracts.Interfaces;

namespace OggettoCase.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class AuthorizationController: ControllerBase
{
    private readonly IGoogleService _googleService;
    private readonly IUserService _userService;
    
    public AuthorizationController(IGoogleService googleService, IUserService userService)
    {
        _googleService = googleService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<UserDto> AuthorizeUser([FromBody] AuthorizeUserDto authorizeUserDto, CancellationToken ct)
    {
        var userAuthInfo = await _googleService.AuthorizeUserAsync(authorizeUserDto.AccessToken, ct);
        var user = await _userService.GetUserByEmailAsync(userAuthInfo.Email, ct);
        if (user is not null)
        {
            return user;
        }
        
        var createUserParams = new CreateUserDto(
            userAuthInfo.Email,
            userAuthInfo.Name,
            userAuthInfo.Surname,
            userAuthInfo.PictureUrl,
            UserAuthenticationTypeEnumDto.Google,
            authorizeUserDto.AccessToken
        );
        user = await _userService.CreateUserAsync(createUserParams, ct);

        return user;
    }
}


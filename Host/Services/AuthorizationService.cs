using System.Security.Claims;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;

namespace OggettoCase.Services;

public class AuthorizationService: IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="tokenGenerator"></param>
    public AuthorizationService(IUserRepository userRepository, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }

    ///<inheritdoc />
    public async Task<string> GetAccessTokenAsync(string email, string refreshToken,  CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(email, ct);
        if (user is null)
        {
            throw new KeyNotFoundException($"User with this email: '{email}' cannot be found");
        }

        if (!user.RefreshToken.Equals( refreshToken, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Refresh tokens does not match");
        }

        
        var jwt = _tokenGenerator.GenerateJwt(user);
        return jwt;
    }
    
    ///<inheritdoc />
    public async Task<string> RefreshTokenAsync(string refreshToken, string email, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(email, ct);
        if (user is null)
        {
            throw new KeyNotFoundException($"User with email: '{email}' cannot be found");
        }
        CheckRefreshToken(refreshToken, user);

        return _tokenGenerator.GenerateJwt(user);
    }
    
    private static void CheckRefreshToken(string refreshToken, User user)
    {
        if (!user.RefreshToken.Equals(refreshToken))
        {
            throw new KeyNotFoundException("Invalid refresh token has been provided.");
        }

        if (user.RefreshTokenExpirationDate < DateTimeOffset.Now)
        {
            throw new InvalidDataException("Refresh token is expired.");
        }
    }
}
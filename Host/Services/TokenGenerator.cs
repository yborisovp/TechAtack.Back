using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OggettoCase.Configuration;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataAccess.Models.Users.Enums;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.Interfaces;

namespace OggettoCase.Services;

public class TokenGenerator: ITokenGenerator
{
    
    private readonly JwtConfiguration _jwtOptions;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="jwtOptions"></param>
    public TokenGenerator(IOptions<JwtConfiguration> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    private const string DefaultSecurityKey = "this is my custom Secret key for authentication";
    
    
    public string GenerateJwt(User user, string externalAccessToken = "")
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key.IsNullOrEmpty() ? DefaultSecurityKey : _jwtOptions.Key));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new ("userId", user.Id.ToString()),
            new ("oauthToken", externalAccessToken),
            new ("name", user.Name + " " + user.Surname),
        };
        if (!user.IsApproved)
        {
            claims.Add(new Claim(ClaimTypes.Role, RoleNamesConstants.NotApproved));
        }
        else
        {
            switch (user.Role)
            {
                case UserRoleEnum.Admin:
                    claims.Add(new Claim(ClaimTypes.Role, RoleNamesConstants.Admin));
                    break;
                case UserRoleEnum.Specialist:
                    claims.Add(new Claim(ClaimTypes.Role, RoleNamesConstants.Specialist));
                    break;
                case UserRoleEnum.Normal:
                    claims.Add(new Claim(ClaimTypes.Role, RoleNamesConstants.Normal));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        var jwtTokenExpirationTime = DateTimeOffset.Now.AddMinutes(_jwtOptions.JwtTokenExpirationTimeInMinutes);
        
        var jwt = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            jwtTokenExpirationTime.DateTime,
            signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }

    public void RevokeToken()
    {
        var jwtTokenExpirationTime = DateTimeOffset.Now.AddMinutes(-10);

        var jwt = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            null,
            null,
            jwtTokenExpirationTime.DateTime);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            TokenExpirationDate = DateTime.UtcNow.AddHours(_jwtOptions.RefreshTokenExpirationTimeInHours)
        };
    }
}
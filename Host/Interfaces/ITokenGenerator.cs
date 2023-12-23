using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Authorization;

namespace OggettoCase.Interfaces;


public interface ITokenGenerator
{
    string GenerateJwt(User user, string externalAccessToken = "");
    
    RefreshToken GenerateRefreshToken();

    void RevokeToken();
}
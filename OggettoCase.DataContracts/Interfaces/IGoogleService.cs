using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Interfaces;

public interface IGoogleService
{
    public Task<UserAuthorizationData> AuthorizeUserAsync(string authorizationToken, CancellationToken ct = default);
}
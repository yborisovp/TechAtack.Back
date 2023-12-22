namespace OggettoCase.DataContracts.Dtos.Authorization;

public class AuthorizeUserDto
{
    public required string AccessToken { get; set; }
    public required string ClientId { get; set; }
}
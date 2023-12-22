using OggettoCase.DataContracts.Dtos.Users.Enums;

namespace OggettoCase.DataContracts.Dtos.Users;

public record CreateUserDto(string Email, string Name, string Surname, string PictureUrl, UserAuthenticationTypeEnumDto AuthenticationType, string AccessToken );

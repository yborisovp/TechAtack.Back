using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Dtos;

public record CreateUserEntityDto(string ExternalId, string Email, string Name, string Surname, string PictureUrl, UserAuthenticationTypeEnum AuthenticationType, string AccessToken );

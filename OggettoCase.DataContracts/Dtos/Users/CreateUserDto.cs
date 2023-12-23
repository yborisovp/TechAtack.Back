using System.ComponentModel.DataAnnotations;
using OggettoCase.DataContracts.Dtos.Users.Enums;

namespace OggettoCase.DataContracts.Dtos.Users;

public record CreateUserDto([Required]string ExternalId, [Required] string Email, [Required]string Name, [Required]string Surname, [Required]string PictureUrl, UserAuthenticationTypeEnumDto AuthenticationType, string AccessToken );

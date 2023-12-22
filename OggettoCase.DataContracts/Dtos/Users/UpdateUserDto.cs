using System.ComponentModel.DataAnnotations;
using OggettoCase.DataContracts.Dtos.Users.Enums;

namespace OggettoCase.DataContracts.Dtos.Users;

public class UpdateUserDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public UserRoleEnumDto Role { get; set; }

    public UserAuthenticationTypeEnumDto AuthenticationType { get; set; }
}
using ServiceTemplate.DataContracts.Dtos.Templates.Enums;
using ServiceTemplate.DataContracts.Dtos.Users.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceTemplate.DataContracts.Dtos.Users;

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
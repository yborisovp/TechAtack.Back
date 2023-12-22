using ServiceTemplate.DataContracts.Dtos.Users.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceTemplate.DataContracts.Dtos.Users;

public class UserDto
{
    public required long Id { get; set; }

    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Surname { get; set; } = string.Empty;

    public string FullName
    { get { return Name + " " + Surname; } }

    [EmailAddress]
    [MaxLength(150)]
    public required string Email { get; set; }

    public UserRoleEnumDto Role { get; set; }

    public UserAuthenticationTypeEnumDto AuthenticationType { get; set; }
}
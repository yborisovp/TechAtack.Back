using System.ComponentModel.DataAnnotations;
using OggettoCase.DataContracts.Dtos.Users.Enums;

namespace OggettoCase.DataContracts.Dtos.Users;

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
    
    public required string PhotoUrl { get; set; }

    public UserRoleEnumDto Role { get; set; }

    public UserAuthenticationTypeEnumDto AuthenticationType { get; set; }
    public bool IsApproved { get; set; }
}
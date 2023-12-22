using ServiceTemplate.DataAccess.Models.Users.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceTemplate.DataAccess.Models.Users;

public class User
{
    public required long Id { get; set; }

    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Surname { get; set; } = string.Empty;

    public UserRoleEnum Role { get; set; }

    [MaxLength(150)]
    public required string Email { get; set; }

    public UserAuthenticationTypeEnum AuthenticationType { get; set; }
}
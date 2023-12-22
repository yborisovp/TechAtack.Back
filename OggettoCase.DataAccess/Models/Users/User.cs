using System.ComponentModel.DataAnnotations;
using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Models.Users;

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
    
    public string AccessToken { get; set; }
    
    public bool IsApproved { get; set; } = false;
}
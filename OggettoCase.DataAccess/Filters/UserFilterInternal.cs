using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Filters;

public class UserFilterInternal
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email {get;set;}
    public bool? ApprovedState { get; set; }
    public UserRoleEnum? Role { get; set; }
}
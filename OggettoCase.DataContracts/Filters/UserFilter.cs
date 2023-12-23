using OggettoCase.DataContracts.Dtos.Users.Enums;

namespace OggettoCase.DataContracts.Filters;

public class UserFilter
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email {get;set;}
    public bool? ApprovedState { get; set; }
    public UserRoleEnumDto? Role { get; set; }
}
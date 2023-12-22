using ServiceTemplate.DataAccess.Models.Users;
using ServiceTemplate.DataContracts.Dtos.Users;

namespace ServiceTemplate.Mappers.Users;

/// <summary>
/// Templates Mapper from dto to db entity model
/// </summary>
public static class DbToDtoUserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Role = user.Role.ToDto(),
            AuthenticationType = user.AuthenticationType.ToDto()
        };
    }
}
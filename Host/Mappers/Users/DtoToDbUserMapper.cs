using ServiceTemplate.DataAccess.Models.Users;
using ServiceTemplate.DataContracts.Dtos.Users;
using ServiceTemplate.Mappers.Users;

namespace ServiceTemplate.Mappers.Templates;

/// <summary>
/// Templates Mapper fromm db entity model to dto
/// </summary>
public static class DtoToDbUserMapper
{
    /// <summary>
    /// Convert database template to dto
    /// </summary>
    /// <param name="updatedUserDto">Updated entity</param>
    /// <param name="templateId">id of template</param>
    /// <returns></returns>
    public static User ToEntity(this UpdateUserDto updatedUserDto, long userId)
    {
        return new User
        {
            Id = userId,
            Name = updatedUserDto.Name,
            Surname = updatedUserDto.Surname,
            Email = updatedUserDto.Email,
            Role = updatedUserDto.Role.ToEntity(),
            AuthenticationType = updatedUserDto.AuthenticationType.ToEntity()
        };
    }
}
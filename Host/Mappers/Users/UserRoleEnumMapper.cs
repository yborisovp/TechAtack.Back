using ServiceTemplate.DataAccess.Models.Users.Enums;
using ServiceTemplate.DataContracts.Dtos.Users.Enums;

namespace ServiceTemplate.Mappers.Users;

/// <summary>
/// Mapping Template enum types to dto and reverse
/// </summary>
public static class UserRoleEnumMapper
{
    /// <summary>
    /// Mapping db enum to dto enum
    /// </summary>
    /// <param name="templateEnum">Enum type</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If values doesn't exists provides an mapping error</exception>
    public static DataContracts.Dtos.Users.Enums.UserRoleEnumDto ToDto(this UserRoleEnum templateEnum)
    {
        return templateEnum switch
        {
            UserRoleEnum.Admin => DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Admin,
            UserRoleEnum.Specialist => DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Specialist,
            UserRoleEnum.Normal => DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Normal,
            _ => throw new ArgumentOutOfRangeException(nameof(templateEnum), templateEnum, null)
        };
    }
    
    /// <summary>
    /// Mapping dto enum to db enum
    /// </summary>
    /// <param name="templateEnum">DTO enum type</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If values doesn't exists provides an mapping error</exception>
    public static UserRoleEnum ToEntity(this DataContracts.Dtos.Users.Enums.UserRoleEnumDto templateEnum)
    {
        return templateEnum switch
        {
            DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Admin => UserRoleEnum.Admin,
            DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Specialist => UserRoleEnum.Specialist,
            DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Normal => UserRoleEnum.Normal,
            _ => throw new ArgumentOutOfRangeException(nameof(templateEnum), templateEnum, null)
        };
    }
}
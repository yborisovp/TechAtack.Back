using OggettoCase.DataAccess.Models.Users.Enums;
using OggettoCase.DataContracts.Dtos.Users.Enums;

namespace OggettoCase.Mappers.Users;

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
    public static OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto ToDto(this UserRoleEnum templateEnum)
    {
        return templateEnum switch
        {
            UserRoleEnum.Admin => OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Admin,
            UserRoleEnum.Specialist => OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Specialist,
            UserRoleEnum.Normal => OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Normal,
            _ => throw new ArgumentOutOfRangeException(nameof(templateEnum), templateEnum, null)
        };
    }
    
    /// <summary>
    /// Mapping dto enum to db enum
    /// </summary>
    /// <param name="templateEnum">DTO enum type</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If values doesn't exists provides an mapping error</exception>
    public static UserRoleEnum ToEntity(this OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto templateEnum)
    {
        return templateEnum switch
        {
            OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Admin => UserRoleEnum.Admin,
            OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Specialist => UserRoleEnum.Specialist,
            OggettoCase.DataContracts.Dtos.Users.Enums.UserRoleEnumDto.Normal => UserRoleEnum.Normal,
            _ => throw new ArgumentOutOfRangeException(nameof(templateEnum), templateEnum, null)
        };
    }
}
using ServiceTemplate.DataAccess.Models.Users.Enums;
using ServiceTemplate.DataContracts.Dtos.Users.Enums;

namespace ServiceTemplate.Mappers.Users;

/// <summary>
/// Mapping Template enum types to dto and reverse
/// </summary>
public static class UserAuthenticationTypeEnumMapper
{
    /// <summary>
    /// Mapping db enum to dto enum
    /// </summary>
    /// <param name="templateEnum">Enum type</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If values doesn't exists provides an mapping error</exception>
    public static UserAuthenticationTypeEnumDto ToDto(this UserAuthenticationTypeEnum templateEnum)
    {
        return templateEnum switch
        {
            UserAuthenticationTypeEnum.Google => UserAuthenticationTypeEnumDto.Google,
            _ => throw new ArgumentOutOfRangeException(nameof(templateEnum), templateEnum, null)
        };
    }
    
    /// <summary>
    /// Mapping dto enum to db enum
    /// </summary>
    /// <param name="templateEnum">DTO enum type</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If values doesn't exists provides an mapping error</exception>
    public static UserAuthenticationTypeEnum ToEntity(this UserAuthenticationTypeEnumDto templateEnum)
    {
        return templateEnum switch
        {
            UserAuthenticationTypeEnumDto.Google => UserAuthenticationTypeEnum.Google,
            _ => throw new ArgumentOutOfRangeException(nameof(templateEnum), templateEnum, null)
        };
    }
}
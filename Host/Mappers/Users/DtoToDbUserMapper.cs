using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Models.Categories;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.Mappers.Users;

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
    public static User ToEntity(this UpdateUserDto updatedUserDto, long userId, Category category)
    {
        return new User
        {
            Id = userId,
            Name = updatedUserDto.Name,
            Surname = updatedUserDto.Surname,
            Email = "",
            Role = updatedUserDto.Role.ToEntity(),
            Category = category,
            PhotoUrl = ""
        };
    }

    public static CreateUserEntityDto ToEntity(this CreateUserDto createUserDto)
    {
        return new CreateUserEntityDto(
            createUserDto.ExternalId,
            createUserDto.Email,
            createUserDto.Name,
            createUserDto.Surname,
            createUserDto.PictureUrl,
            createUserDto.AuthenticationType.ToEntity(),
            createUserDto.AccessToken
        );
    }
}
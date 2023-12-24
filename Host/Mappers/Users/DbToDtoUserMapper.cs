using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.Mappers.Users;

/// <summary>
/// Templates Mapper from dto to db entity model
/// </summary>
public static class DbToDtoUserMapper
{
    public static UserDto ToDto(this User? user)
    {
        if (user == null)
        {
            return null;
        }
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Role = user.Role.ToDto(),
            AuthenticationType = user.AuthenticationType.ToDto(),
            PhotoUrl = user.PhotoUrl,
            IsApproved = user.IsApproved,
            Category = user.Category?.Description
        };
    }
}
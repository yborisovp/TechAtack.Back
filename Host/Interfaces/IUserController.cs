using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.Interfaces;

/// <summary>
/// Controller to grant access to Users
/// </summary>
public interface IUserController : IBaseController<UserDto, long, UpdateUserDto>
{
}
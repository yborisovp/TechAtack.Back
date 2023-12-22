using ServiceTemplate.DataContracts.Dtos.Users;

namespace ServiceTemplate.Interfaces;

/// <summary>
/// Controller to grant access to Users
/// </summary>
public interface IUserController : IBaseController<UserDto, long, UpdateUserDto>
{
}
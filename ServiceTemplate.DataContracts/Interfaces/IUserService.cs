using ServiceTemplate.DataContracts.Dtos.Users;

namespace ServiceTemplate.DataContracts.Interfaces;

public interface IUserService : IBaseService<UserDto, long, UpdateUserDto>
{
}
using ServiceTemplate.DataAccess.Models.Users;

namespace ServiceTemplate.DataAccess.Interfaces;

public interface IUserRepository : IRepository<User, long>
{
}
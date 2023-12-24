using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Filters;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Interfaces;

public interface IUserRepository : IRepository<User, long>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User> CreateUserAsync(CreateUserEntityDto createUserEntityParams, CancellationToken ct);
    Task ApproveUserAccountAsync(long userId, UserRoleEnum aprrovedRole, CancellationToken ct);
    Task<IEnumerable<User>> GetByFilterAsync(UserFilterInternal filter, CancellationToken ct);
    Task<List<User>> GetSeveralByIdAsync(IList<long> userIds, CancellationToken ct);
}
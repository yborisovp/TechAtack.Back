using OggettoCase.DataContracts.Dtos.Category;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.Interfaces;

/// <summary>
/// Controller to grant access to Users
/// </summary>
public interface ICategoryController : IBaseController<CategoryDto, int, CategoryDto>
{
}
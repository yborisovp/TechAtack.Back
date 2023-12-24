using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Filters;
using OggettoCase.DataAccess.Models.Categories;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Interfaces;

public interface ICategoryRepository : IRepository<Category, long>
{
    public Task<Category> CreateCategoryAsync(string categoryDescriptions, CancellationToken ct = default);
}
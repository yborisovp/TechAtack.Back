using OggettoCase.DataContracts.Dtos.Category;

namespace OggettoCase.DataContracts.Interfaces;

public interface ICategoryService: IBaseService<CategoryDto, long, CategoryDto>
{
    public Task<CategoryDto> CreateCategoryAsync(string categoryDescriptions, CancellationToken ct = default);
}
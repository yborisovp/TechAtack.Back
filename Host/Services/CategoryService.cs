using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Category;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Dtos.Users.Enums;
using OggettoCase.DataContracts.Filters;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using OggettoCase.Mappers.Categories;
using OggettoCase.Mappers.Comments;
using OggettoCase.Mappers.Filters;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Services;

/// <summary>
/// Service to grant access to db entities
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;
    private readonly ITokenGenerator _tokenGenerator;
    
    /// <summary>
    /// Constructor of an service
    /// </summary>
    /// <param name="categoryRepository"></param>
    /// <param name="logger"></param>
    public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, ITokenGenerator tokenGenerator)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
        _tokenGenerator = tokenGenerator;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(CategoryDto));

        var categorys = await _categoryRepository.GetAllAsync(ct);

        _logger.LogDebug("Successfully received list of {name of}", nameof(CategoryDto));
        return categorys.Select(DbToDtoCategoryMapper.ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<CategoryDto> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} with id: '{id}'", nameof(CategoryDto), id);
        var category = await _categoryRepository.GetByIdAsync(id, ct);

        if (category is null)
        {
            throw new KeyNotFoundException($"{nameof(CategoryDto)} with id: '{id}' doesn't exist");
        }

        _logger.LogDebug("Successfully received {name of} with id: '{id}'", nameof(CategoryDto), id);
        return category.ToDto();
    }

    /// <inheritdoc />
    public async Task<CategoryDto> UpdateByIdAsync(long id, CategoryDto dtoToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}'", nameof(CategoryDto), id);
        var category = await _categoryRepository.GetByIdAsync(id, ct);

        if (category is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while update", nameof(CategoryDto), id);
            throw new KeyNotFoundException($"{nameof(CategoryDto)} with id: '{id}' doesn't exist");
        }

        var categoryToUpdate = dtoToUpdate.ToEntity();

        var updatedCategory = await _categoryRepository.UpdateAsync(categoryToUpdate, ct);
        if (updatedCategory is null)
        {
            _logger.LogError("Cannot update {name of} with id: '{id}'", nameof(CategoryDto), id);
            throw new InvalidProgramException($"Cannot update {nameof(CategoryDto)} with id: '{id}'");
        }

        var result = await _categoryRepository.GetByIdAsync(id, ct);

        _logger.LogDebug("Successfully updated {name of} with id: '{id}'", nameof(CategoryDto), id);
        return result.ToDto();
    }

    /// <inheritdoc />
    public async Task<long> DeleteByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}'", nameof(CategoryDto), id);
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while deleting", nameof(CategoryDto), id);
            throw new KeyNotFoundException($"{nameof(CategoryDto)} with id: '{id}' cannot be deleted");
        }

        var deletedId = await _categoryRepository.DeleteByIdAsync(category.Id, ct);

        _logger.LogDebug("Successfully delete {name of} with id: '{id}'", nameof(CategoryDto), id);
        return deletedId;
    }
    

    public async Task<CategoryDto> CreateCategoryAsync(string categoryDescriptions, CancellationToken ct = default)
    {
        var category = await _categoryRepository.CreateCategoryAsync(categoryDescriptions, ct);
        
        return category.ToDto();
    }
}
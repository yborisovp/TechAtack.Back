using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataContracts.Dtos.Category;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Dtos.Users.Enums;
using OggettoCase.DataContracts.Filters;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using OggettoCase.Mappers.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace OggettoCase.Controllers;

/// <summary>
/// API to control categories
/// </summary>
[ApiController]
[Authorize(Roles = "admin")]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class CategoryController : ControllerBase, ICategoryController
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    /// <summary>
    /// Constructor of CategoryController
    /// </summary>
    /// <param name="categoryService"></param>
    /// <param name="logger"></param>
    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <inheritdoc />
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation($"Get all {nameof(CategoryDto)}s")]
    [SwaggerResponse(200, type: typeof(IEnumerable<CategoryDto>), description: $"List of {nameof(CategoryDto)}s")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(CategoryDto));
        var categoryDtos = await _categoryService.GetAllAsync(ct);
        _logger.LogDebug("Successfully received list of {CategoryDto}s", nameof(CategoryDto));
        return Ok(categoryDtos);
    }

    /// <inheritdoc />
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [SwaggerOperation($"Get one {nameof(CategoryDto)}")]
    [SwaggerResponse(200, type: typeof(CategoryDto), description: $"Receive one {nameof(CategoryDto)} by id")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CategoryDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<CategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} with id: '{id}'", nameof(CategoryDto), id);
        var category = await _categoryService.GetByIdAsync(id, ct);
        _logger.LogDebug("Successfully received one {CategoryDto} by id: '{id}'", nameof(CategoryDto), id);
        return Ok(category);
    }

    /// <inheritdoc />
    [HttpPut("{id:Guid}")]
    [SwaggerOperation($"Update {nameof(CategoryDto)}")]
    [SwaggerResponse(200, type: typeof(CategoryDto), description: $"{nameof(CategoryDto)} successfully updated")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CategoryDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<CategoryDto>> UpdateByIdAsync(int id, CategoryDto dtoToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}''", nameof(CategoryDto), id);
        var categoryUpdated = await _categoryService.UpdateByIdAsync(id, dtoToUpdate, ct);
        _logger.LogDebug("Successfully update {CategoryDto} by id: '{id}'", nameof(CategoryDto), id);

        return Ok(categoryUpdated);
    }

    /// <inheritdoc />
    [HttpDelete("{id:Guid}")]
    [SwaggerOperation($"Delete {nameof(CategoryDto)}")]
    [SwaggerResponse(200, type: typeof(CategoryDto), description: $"{nameof(CategoryDto)} successfully deleted")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CategoryDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<int>> DeleteByIdAsync(int id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}'", nameof(CategoryDto), id);
        var categoryDeletedId = await _categoryService.DeleteByIdAsync(id, ct);
        _logger.LogDebug("Successfully delete {CategoryDto} by id: '{id}'", nameof(CategoryDto), id);

        return Ok(categoryDeletedId);
    }
}
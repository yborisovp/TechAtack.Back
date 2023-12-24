using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OggettoCase.DataAccess.Context;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Categories;

namespace OggettoCase.DataAccess.Repositories;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(IDatabaseContextFactory contextFactory, ILogger<CategoryRepository> logger) : base(contextFactory)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(Category));
        await using var context = ContextFactory.CreateDbContext();

        var templates = await GetFullQuery(context.Categories)
            .ToListAsync(ct);

        return templates;
    }

    /// <inheritdoc />
    public async Task<Category> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} by Id: '{templateId}' from database", nameof(Category), id);
        await using var context = ContextFactory.CreateDbContext();

        var template = await context.Categories.Where(t => t.Id == id).SingleOrDefaultAsync(ct);

        if (template is null)
        {
            throw new KeyNotFoundException($"{nameof(Category)} with Id: '{id}' not found");
        }

        return template;
    }

    /// <inheritdoc />
    public async Task<Category> UpdateAsync(Category entityToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with Id: '{id}' in database", nameof(Category), entityToUpdate.Id);

        await using var context = ContextFactory.CreateDbContext();
        
        context.Categories.Update(entityToUpdate);
        await context.SaveChangesAsync(ct);

        _logger.LogDebug("{name of} with Id: '{id}' - updated in database", nameof(Category), entityToUpdate.Id);

        return entityToUpdate;
    }

    /// <inheritdoc />
    public async Task<long> DeleteByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}' in the database", nameof(Category), id);
        await using var context = ContextFactory.CreateDbContext();

        var entity = await context.Categories.Where(e => e.Id == id).FirstOrDefaultAsync(ct);
        if (entity != null)
        {
            context.Categories.Remove(entity);
        }
        else
        {
            throw new KeyNotFoundException($"{nameof(Category)} with provided id: '{id}' - not found in database");
        }

        await context.SaveChangesAsync(ct);

        _logger.LogDebug("{name of} with id {id} was deleted successfully", nameof(Category), id);
        return id;
    }

    public async Task<Category> CreateCategoryAsync(string categoryDescriptions, CancellationToken ct = default)
    {
        _logger.LogDebug("Create {name of}'.", nameof(Category));
        await using var context = ContextFactory.CreateDbContext();
        var category = new Category()
        {
            Description = categoryDescriptions
        };

        var userEntity = await context.Categories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);
        return userEntity.Entity;
    }


    private static IQueryable<Category> GetFullQuery(IQueryable<Category> form)
    {
        //Remove when fix https://github.com/dotnet/efcore/issues/21663
#pragma warning disable CS8620
        return form;
#pragma warning restore CS8620
    }
}
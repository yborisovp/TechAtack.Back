using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceTemplate.DataAccess.Context;
using ServiceTemplate.DataAccess.Interfaces;
using ServiceTemplate.DataAccess.Models.Users;

namespace ServiceTemplate.DataAccess.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly ILogger<TemplateRepository> _logger;

    public UserRepository(IDatabaseContextFactory contextFactory, ILogger<TemplateRepository> logger) : base(contextFactory)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(User));
        await using var context = ContextFactory.CreateDbContext();

        var templates = await GetFullQuery(context.Users)
            .ToListAsync(ct);

        return templates;
    }

    /// <inheritdoc />
    public async Task<User> GetByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} by Id: '{templateId}' from database", nameof(User), id);
        await using var context = ContextFactory.CreateDbContext();

        var template = await context.Users.Where(t => t.Id == id).FirstOrDefaultAsync(ct);

        if (template is null)
        {
            throw new KeyNotFoundException($"{nameof(User)} with Id: '{id}' not found");
        }

        return template;
    }

    /// <inheritdoc />
    public async Task<User> UpdateAsync(User entityToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with Id: '{id}' in database", nameof(User), entityToUpdate.Id);

        await using var context = ContextFactory.CreateDbContext();
        context.Users.Update(entityToUpdate);
        await context.SaveChangesAsync(ct);

        _logger.LogDebug("{name of} with Id: '{id}' - updated in database", nameof(User), entityToUpdate.Id);

        return entityToUpdate;
    }

    /// <inheritdoc />
    public async Task<long> DeleteByIdAsync(long id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}' in the database", nameof(User), id);
        await using var context = ContextFactory.CreateDbContext();

        var entity = await context.Users.Where(e => e.Id == id).FirstOrDefaultAsync(ct);
        if (entity != null)
        {
            context.Users.Remove(entity);
        }
        else
        {
            throw new KeyNotFoundException($"{nameof(User)} with provided id: '{id}' - not found in database");
        }

        await context.SaveChangesAsync(ct);

        _logger.LogDebug("{name of} with id {id} was deleted successfully", nameof(User), id);
        return id;
    }

    private static IQueryable<User> GetFullQuery(IQueryable<User> form)
    {
        //Remove when fix https://github.com/dotnet/efcore/issues/21663
#pragma warning disable CS8620
        return form;
#pragma warning restore CS8620
    }
}
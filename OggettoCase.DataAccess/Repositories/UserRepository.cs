using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OggettoCase.DataAccess.Context;
using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Filters;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IDatabaseContextFactory contextFactory, ILogger<UserRepository> logger) : base(contextFactory)
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

        var template = await context.Users.Where(t => t.Id == id).SingleOrDefaultAsync(ct);

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
        var entry = context.Entry(entityToUpdate);
        entry.Property(x => x.IsApproved).IsModified = false;
        entry.Property(x => x.PhotoUrl).IsModified = false;
        entry.Property(x => x.Id).IsModified = false;
        entry.Property(x => x.AccessToken).IsModified = false;
        entry.Property(x => x.ExternalId).IsModified = false;
        entry.Property(x => x.Email).IsModified = false;
        entry.Property(x => x.RefreshTokenExpirationDate).IsModified = false;
        entry.Property(x => x.RefreshToken).IsModified = false;
            //entry.Property(x => x.CalendarEvents).IsModified = false;
        entry.Property(x => x.AuthenticationType).IsModified = false;
        
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

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} by Email: '{email}' from database.", nameof(User), email);
        await using var context = ContextFactory.CreateDbContext();

        var template = await context.Users.Where(t => t.Email == email).FirstOrDefaultAsync(ct);

        if (template is null)
        {
            _logger.LogDebug("{nameof(User)} with Email: '{email}' not found.", nameof(User), email);
        }

        return template;
    }

    public async Task<User> CreateUserAsync(CreateUserEntityDto createUserEntityParams, CancellationToken ct = default)
    {
        _logger.LogDebug("Create {name of} with Email: '{email}'.", nameof(User), createUserEntityParams.Email);
        await using var context = ContextFactory.CreateDbContext();
        var user = new User
        {
            Id = 0,
            ExternalId = createUserEntityParams.ExternalId,
            Name = createUserEntityParams.Name,
            Surname = createUserEntityParams.Surname,
            Role = UserRoleEnum.Normal,
            Email = createUserEntityParams.Email,
            AuthenticationType = createUserEntityParams.AuthenticationType,
            AccessToken = createUserEntityParams.AccessToken,
            IsApproved = false,
            PhotoUrl = createUserEntityParams.PictureUrl
        };

        var userEntity = await context.Users.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
        return userEntity.Entity;
    }

    public async Task ApproveUserAccountAsync(long userId, UserRoleEnum approvedRole, CancellationToken ct)
    {
        await using var context = ContextFactory.CreateDbContext();
        var user = await context.Users.Where(x => x.Id == userId).SingleAsync(ct);
        user.IsApproved = true;
        user.Role = approvedRole;
        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<User>> GetByFilterAsync(UserFilterInternal filter, CancellationToken ct)
    {
        await using var context = ContextFactory.CreateDbContext();
        var query = context.Users.AsQueryable();
        if (filter.Name != null)
        {
            query = query.Where(x => x.Name.Contains(filter.Name));
        }
        
        if (filter.Surname != null)
        {
            query = query.Where(x => x.Surname.Contains(filter.Surname));
        }
        
        if (filter.Email != null)
        {
            query = query.Where(x => x.Email.Contains(filter.Email));
        }
        
        if (filter.ApprovedState != null)
        {
            query = query.Where(x => x.IsApproved == filter.ApprovedState);
        }
        
        if (filter.Role != null)
        {
            query = query.Where(x => x.Role == filter.Role);
        }

        var result = await query.ToListAsync(ct);
        return result;
    }

    public async Task<List<User>> GetSeveralByIdAsync(IList<long> userIds, CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(User));
        await using var context = ContextFactory.CreateDbContext();

        var templates = await GetFullQuery(context.Users).Where(u => userIds.Contains(u.Id))
            .ToListAsync(ct);

        return templates;
    }

    private static IQueryable<User> GetFullQuery(IQueryable<User> form)
    {
        //Remove when fix https://github.com/dotnet/efcore/issues/21663
#pragma warning disable CS8620
        return form;
#pragma warning restore CS8620
    }
}
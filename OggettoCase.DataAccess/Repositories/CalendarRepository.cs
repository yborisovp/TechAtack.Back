using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OggettoCase.DataAccess.Context;
using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Filters;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Models.Users;

namespace OggettoCase.DataAccess.Repositories;

public class CalendarRepository : BaseRepository, ICalendarRepository
{
    private readonly ILogger<CalendarRepository> _logger;

    public CalendarRepository(IDatabaseContextFactory contextFactory, ILogger<CalendarRepository> logger) : base(contextFactory)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Calendar>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(Calendar));
        await using var context = ContextFactory.CreateDbContext();

        var templates = await GetFullQuery(context.Calendars)
            .ToListAsync(ct);

        return templates;
    }

    /// <inheritdoc />
    public async Task<Calendar> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} by Id: '{templateId}' from database", nameof(Calendar), id);
        await using var context = ContextFactory.CreateDbContext();

        var template = await context.Calendars.Where(t => t.Id == id).FirstOrDefaultAsync(ct);

        if (template is null)
        {
            throw new KeyNotFoundException($"{nameof(Calendar)} with Id: '{id}' not found");
        }

        return template;
    }

    /// <inheritdoc />
    public async Task<Calendar> UpdateAsync(Calendar entityToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with Id: '{id}' in database", nameof(Calendar), entityToUpdate.Id);

        await using var context = ContextFactory.CreateDbContext();
        var entry = context.Entry(entityToUpdate);
        entry.Property(x => x.CreatedAt).IsModified = false;
        
        entry.Property(x => x.OwnerId).IsModified = false;
        entry.Property(x => x.LinkToMeeting).IsModified = false;
        context.Calendars.Update(entityToUpdate);
        await context.SaveChangesAsync(ct);

        _logger.LogDebug("{name of} with Id: '{id}' - updated in database", nameof(Calendar), entityToUpdate.Id);

        return entityToUpdate;
    }

    /// <inheritdoc />
    public async Task<Guid> DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}' in the database", nameof(Calendar), id);
        await using var context = ContextFactory.CreateDbContext();

        var entity = await context.Calendars.Where(e => e.Id == id).FirstOrDefaultAsync(ct);
       
        if (entity != null)
        {
            var user = await context.Users.Where(x => x.Id == entity.OwnerId).SingleAsync(ct);
            user.CalendarEvents!.Remove(entity);
            context.Users.Update(user);
            context.Calendars.Remove(entity);
        }
        else
        {
            throw new KeyNotFoundException($"{nameof(Calendar)} with provided id: '{id}' - not found in database");
        }

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (Exception e)
        {
            entity = await context.Calendars.Where(e => e.Id == id).FirstOrDefaultAsync(ct);
            
            if (entity != null)
            {
                entity.IsDeleted = true;
                context.Update(entity);
                await context.SaveChangesAsync(ct);
            }
            else
            {
                throw;
            }
        }

        _logger.LogDebug("{name of} with id {id} was deleted successfully", nameof(Calendar), id);
        return id;
    }

    public async Task<Calendar> UpdateSubscribedUsersAsync(Guid eventId, IEnumerable<long> userIds, CancellationToken ct)
    {
        _logger.LogDebug("Update subscribed users {name of} with Id: '{id}' in database", nameof(Calendar), eventId);

        await using var context = ContextFactory.CreateDbContext();
        var entity = await context.Calendars.Where(x => x.Id == eventId).SingleAsync(ct);
        var users = await context.Users.Where(x => userIds.Contains(x.Id)).ToListAsync(ct);
        entity.Users = users;
        context.Calendars.Update(entity);
        await context.SaveChangesAsync(ct);

        _logger.LogDebug("subscribed users {name of} with Id: '{id}' - updated in database", nameof(Calendar), eventId);

        return entity;
    }

    public async Task<Calendar> CreateCalendarAsync(CreateCalendarEntityDto createCalendarEntityParams, string calendarId, string eventId, CancellationToken ct = default)
    {
        _logger.LogDebug("Create {name of}.", nameof(Calendar));
        await using var context = ContextFactory.CreateDbContext();
        var users = new List<User>();

        if (createCalendarEntityParams.UserIds is not null)
        {
            users = await context.Users.Where(x => createCalendarEntityParams.UserIds.Contains(x.Id)).ToListAsync(ct);
        }
        
        var calendar = new Calendar
        {
            Title = createCalendarEntityParams.Title,
            Description = createCalendarEntityParams.Description,
            CreatedAt = DateTime.UtcNow,
            StartedAt = createCalendarEntityParams.StartedAt,
            EndedAt = createCalendarEntityParams.EndedAt,
            OwnerId = createCalendarEntityParams.OwnerId,
            Users = users,
            LinkToMeeting = createCalendarEntityParams.LinkToMeeting,
            ExternalCalendarId = calendarId,
            ExternalEventId = eventId,
            AdditionalLinks = createCalendarEntityParams.AdditionalLinks,
            EventDetails = createCalendarEntityParams.EventDetails
        };

        var calendarEntity = await context.Calendars.AddAsync(calendar, ct);
        await context.SaveChangesAsync(ct);
        return calendarEntity.Entity;
    }

    public async Task<IEnumerable<Calendar>> GetFilteredEventsAsync(CalendarFilterInternal calendarFilter, CancellationToken ct = default)
    {
        await using var context = ContextFactory.CreateDbContext();
        var query = GetFullQuery(context.Calendars).AsQueryable();
        if (calendarFilter.Title is not null)
        {
            query = query.Union(context.Calendars.Where(c => c.Title.ToLower().Contains(calendarFilter.Title)));
        }

        if (calendarFilter.OwnerName is not null)
        {
            query = query.Where(c => c.Owner.Name.ToLower().Contains(calendarFilter.OwnerName) || c.Owner.Surname.ToLower().Contains(calendarFilter.OwnerName.ToLower()));
        }
        
        if (calendarFilter.StartDate is not null)
        {
            query = query.Where(c => c.StartedAt >= calendarFilter.StartDate  );
        }
        
        if (calendarFilter.EndDate is not null)
        {
            query = query.Where(c => c.StartedAt <= calendarFilter.EndDate);
        }
        
        if (calendarFilter.Category is not null)
        {
            query = query.Where(c => c.Owner.Category != null && c.Owner.Category.Description.ToLower().Equals(calendarFilter.Category.ToLower()));
        }
        
        var result = await query.ToListAsync(ct);

        return result;
    }

    private static IQueryable<Calendar> GetFullQuery(IQueryable<Calendar> calendar)
    {
        //Remove when fix https://github.com/dotnet/efcore/issues/21663
#pragma warning disable CS8620
        return calendar.Include(x => x.Comments)
            .Include(x => x.Owner)
            .Include(x => x.Users)
            .Where(x => x.IsDeleted == false);
#pragma warning restore CS8620
    }
}
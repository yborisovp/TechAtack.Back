using Microsoft.IdentityModel.Tokens;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Filters;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Mappers.Calendars;
using OggettoCase.Mappers.Comments;
using OggettoCase.Mappers.Filters;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Services;

/// <summary>
/// Service to grant access to db entities
/// </summary>
public class CalendarEventService : ICalendarEventService
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly IGoogleService _googleService;
    private readonly ILogger<CalendarEventService> _logger;
    
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Constructor of an service
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="logger"></param>
    public CalendarEventService(ICalendarRepository userRepository, ILogger<CalendarEventService> logger, IGoogleService googleService, IUserRepository userRepository1)
    {
        _calendarRepository = userRepository;
        _logger = logger;
        _googleService = googleService;
        _userRepository = userRepository1;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CalendarDto>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Get all {name of}s", nameof(CalendarDto));

        var calendar = await _calendarRepository.GetAllAsync(ct);

        _logger.LogDebug("Successfully received list of {name of}", nameof(CalendarDto));
        return calendar.Select(DbToDtoCalendarMapper.ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<CalendarDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Get {name of} with id: '{id}'", nameof(CalendarDto), id);
        var user = await _calendarRepository.GetByIdAsync(id, ct);

        if (user is null)
        {
            throw new KeyNotFoundException($"{nameof(CalendarDto)} with id: '{id}' doesn't exist");
        }

        _logger.LogDebug("Successfully received {name of} with id: '{id}'", nameof(CalendarDto), id);
        return user.ToDto();
    }

    /// <inheritdoc />
    public async Task<CalendarDto> UpdateByIdAsync(Guid id, UpdateCalendarDto dtoToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}'", nameof(CalendarDto), id);
        var calendar = await _calendarRepository.GetByIdAsync(id, ct);

        if (calendar is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while update", nameof(CalendarDto), id);
            throw new KeyNotFoundException($"{nameof(CalendarDto)} with id: '{id}' doesn't exist");
        }

        await _googleService.UpdateEventAsync(calendar.ExternalCalendarId, calendar.ExternalEventId, dtoToUpdate, ct);
        var users = await _userRepository.GetSeveralByIdAsync(dtoToUpdate.UserIds, ct);
        
        var entityToUpdate = dtoToUpdate.ToEntity(users, calendar);

        var updated = await _calendarRepository.UpdateAsync(entityToUpdate, ct);
        if (updated is null)
        {
            _logger.LogError("Cannot update {name of} with id: '{id}'", nameof(CalendarDto), id);
            throw new InvalidProgramException($"Cannot update {nameof(CalendarDto)} with id: '{id}'");
        }

        var result = await _calendarRepository.GetByIdAsync(id, ct);

        _logger.LogDebug("Successfully updated {name of} with id: '{id}'", nameof(CalendarDto), id);
        return result.ToDto();
    }

    /// <inheritdoc />
    public async Task<Guid> DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}'", nameof(CalendarDto), id);
        var calendar = await _calendarRepository.GetByIdAsync(id, ct);
        if (calendar is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while deleting", nameof(CalendarDto), id);
            throw new KeyNotFoundException($"{nameof(CalendarDto)} with id: '{id}' cannot be deleted");
        }

        await _googleService.DeleteEventAsync(calendar.ExternalCalendarId, calendar.ExternalEventId, ct);
        var deletedId = await _calendarRepository.DeleteByIdAsync(calendar.Id, ct);

        _logger.LogDebug("Successfully delete {name of} with id: '{id}'", nameof(CalendarDto), id);
        return deletedId;
    }

    /// <summary>
    /// Update list of user that subscribed to event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userIds"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="InvalidProgramException"></exception>
    public async Task<CalendarDto> UpdateSubscribedUsersAsync(Guid eventId, IEnumerable<long> userIds, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}'", nameof(CalendarDto), eventId);
        var user = await _calendarRepository.GetByIdAsync(eventId, ct);

        if (user is null)
        {
            _logger.LogWarning("Impossible to confirm existence of {name of} with id: '{id}' while update", nameof(CalendarDto), eventId);
            throw new KeyNotFoundException($"{nameof(CalendarDto)} with id: '{eventId}' doesn't exist");
        }

        var updatedTemplate = await _calendarRepository.UpdateSubscribedUsersAsync(eventId, userIds, ct);
        if (updatedTemplate is null)
        {
            _logger.LogError("Cannot update {name of} with id: '{id}'", nameof(CalendarDto), eventId);
            throw new InvalidProgramException($"Cannot update {nameof(CalendarDto)} with id: '{eventId}'");
        }

        var result = await _calendarRepository.GetByIdAsync(eventId, ct);

        _logger.LogDebug("Successfully updated {name of} with id: '{id}'", nameof(CalendarDto), eventId);
        return result.ToDto();
    }

    public async Task<CalendarDto> CreateCalendarEventAsync(CreateCalendarEventDto createCalendarEntityDto, CancellationToken ct = default)
    {
        //var calendId = await _googleService.CreateEventInGoogleCalendar(createCalendarEntityDto, ct);
        List<User> calendar = null;
        if (createCalendarEntityDto.UserIds != null)
        {
            calendar = await _userRepository.GetSeveralByIdAsync(createCalendarEntityDto.UserIds, ct);
        }

        var calendarId = "primary";
        var data = await _googleService.CreateGoogleEventAsync(calendarId,  calendar, createCalendarEntityDto, ct);
        createCalendarEntityDto.LinkToMeeting = data.conferenceData.entryPoints.FirstOrDefault()?.uri ?? "";
        var calendarEvent = await _calendarRepository.CreateCalendarAsync(createCalendarEntityDto.ToEntity(), calendarId, data.id, ct);
        return calendarEvent.ToDto();
    }

    public async Task<IEnumerable<CalendarDto>> GetFilteredEventsAsync(CalendarFilter calendarFilter, CancellationToken ct = default)
    {
        var result = await _calendarRepository.GetFilteredEventsAsync(calendarFilter.ToInternalFilter(), ct);
        var enumerable = result.ToList();
        if (!enumerable.Any())
        {
            return Array.Empty<CalendarDto>();
        }
        return enumerable.Select(x => x.ToDto());
    }
}
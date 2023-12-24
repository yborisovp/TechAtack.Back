using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Interfaces;

public interface IGoogleService
{
    public Task<UserAuthorizationData> AuthorizeUserAsync(AuthorizeUserDto authorizeUserDto, CancellationToken ct = default);
    public Task<string> CreateEventInGoogleCalendar(CreateCalendarEventDto createCalendarEntityDto, CancellationToken ct = default);

    public Task<GoogleCalendarEvent> CreateGoogleEventAsync(string calendarId, List<User>? users,
        CreateCalendarEventDto createCalendarEvent, CancellationToken ct = default);

    public Task DeleteEventAsync(string calendarExternalCalendarId, string calendarExternalEventId, CancellationToken ct);
    public Task UpdateEventAsync(string calendarExternalCalendarId, string calendarExternalEventId, UpdateCalendarDto dtoToUpdate, CancellationToken ct);
    Task<GoogleCalendarEvent> GetGoogleEventData(string eventDataExternalCalendarId, string eventDataExternalEventId, CancellationToken ct);
}
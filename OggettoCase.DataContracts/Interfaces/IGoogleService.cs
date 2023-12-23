using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Interfaces;

public interface IGoogleService
{
    public Task<UserAuthorizationData> AuthorizeUserAsync(AuthorizeUserDto authorizeUserDto, CancellationToken ct = default);
    public Task<string> CreateEventInGoogleCalendar(CreateCalendarEventDto createCalendarEntityDto, CancellationToken ct = default);
    public Task<string> GetLinkToGoogleEvent(string calendarId, DateTimeOffset start, DateTimeOffset end, CancellationToken ct = default);

}
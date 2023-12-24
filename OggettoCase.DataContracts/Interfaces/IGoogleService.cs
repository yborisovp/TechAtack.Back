using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Interfaces;

public interface IGoogleService
{
    public Task<UserAuthorizationData> AuthorizeUserAsync(AuthorizeUserDto authorizeUserDto, CancellationToken ct = default);
    public Task<string> CreateEventInGoogleCalendar(CreateCalendarEventDto createCalendarEntityDto, CancellationToken ct = default);

    public Task<string> GetLinkToGoogleEvent(string calendarId, List<User>? users,
        CreateCalendarEventDto createCalendarEvent, CancellationToken ct = default);

}
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Filters;

namespace OggettoCase.DataContracts.Interfaces;

public interface ICalendarEventService: IBaseService<CalendarDto, Guid, UpdateCalendarDto>
{
    public Task<CalendarDto> UpdateSubscribedUsersAsync(Guid eventId, IEnumerable<long> userIds, CancellationToken ct = default);
    public Task<CalendarDto> CreateCalendarEventAsync(CreateCalendarEventDto createCalendarEntityDto, CancellationToken ct = default);
    public Task<IEnumerable<CalendarDto>> GetFilteredEventsAsync(CalendarFilter calendarFilter, CancellationToken ct = default);
}
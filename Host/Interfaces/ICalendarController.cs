using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataContracts.Dtos.Calendars;

namespace OggettoCase.Interfaces;

public interface ICalendarController: IBaseController<CalendarDto, Guid, UpdateCalendarDto>
{
    public Task<CalendarDto> UpdateSubscribedUsers(Guid eventId, IEnumerable<long> userIds, CancellationToken ct = default);
    public Task<ActionResult<CalendarDto>> CreateCalendarEvent([FromBody] CreateCalendarEventDto createCalendarEvent, CancellationToken ct = default);
}
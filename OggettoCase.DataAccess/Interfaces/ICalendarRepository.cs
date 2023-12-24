using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Filters;
using OggettoCase.DataAccess.Models.Calendars;

namespace OggettoCase.DataAccess.Interfaces;

public interface ICalendarRepository: IRepository<Calendar, Guid>
{
    public Task<Calendar> UpdateSubscribedUsersAsync(Guid eventId, IEnumerable<long> userIds, CancellationToken ct);
    public Task<Calendar> CreateCalendarAsync(CreateCalendarEntityDto createCalendarEntityParams, string calendarId, string eventId, CancellationToken ct = default);
    public Task<IEnumerable<Calendar>> GetFilteredEventsAsync(CalendarFilterInternal calendarFilter, CancellationToken ct = default);
}
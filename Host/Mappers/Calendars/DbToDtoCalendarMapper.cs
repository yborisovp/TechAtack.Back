using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Models.Comments;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Comments;
using OggettoCase.Mappers.Comments;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Mappers.Calendars;

/// <summary>
/// Templates Mapper from dto to db entity model
/// </summary>
public static class DbToDtoCalendarMapper
{
    public static CalendarDto ToDto(this Calendar calendar)
    {
        return new CalendarDto
        {
            Id = calendar.Id,
            CreatedAt = calendar.CreatedAt,
            Title = calendar.Title,
            Description = calendar.Description,
            Owner = calendar.Owner.ToDto(),
            Users = calendar.Users?.Select(x => x.ToDto()).ToList(),
            Comments = calendar.Comments?.Select(x => x.ToDto()).ToList(),
            LinkToMeeting = calendar.LinkToMeeting,
            StartedAt = calendar.StartedAt,
            EndedAt = calendar.EndedAt,
            AdditionalLinks = calendar.AdditionalLinks,
            EventDetails = calendar.EventDetails
        };
    }
}
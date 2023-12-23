using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Mappers.Calendars;

/// <summary>
/// Templates Mapper fromm db entity model to dto
/// </summary>
public static class DtoToDbCalendarMapper
{
    /// <summary>
    /// Convert database template to dto
    /// </summary>
    /// <param name="updatedUserDto">Updated entity</param>
    /// <param name="templateId">id of template</param>
    /// <returns></returns>
    public static Calendar ToEntity(this UpdateCalendarDto updateCalendarDto)
    {
        return new Calendar
        {
            Id = updateCalendarDto.Id,
            Title = updateCalendarDto.Title,
            Description = updateCalendarDto.Description,
        };
    }

    public static CreateCalendarEntityDto ToEntity(this CreateCalendarEventDto createUserDto)
    {
        return new CreateCalendarEntityDto
        {
            OwnerId = createUserDto.OwnerId,
            Title = createUserDto.Title,
            Description = createUserDto.Description,
            StartedAt = createUserDto.StartedAt,
            EndedAt = createUserDto.EndedAt,
            UserIds = createUserDto.UserIds,
            LinkToMeeting = createUserDto.LinkToMeeting
        };
    }
}
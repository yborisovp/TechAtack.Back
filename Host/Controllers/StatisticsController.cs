using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataContracts.Dtos.Statistics;
using OggettoCase.DataContracts.Interfaces;

namespace OggettoCase.Controllers;

/// <summary>
/// Controller to with with statistics
/// </summary>
[ApiController]
[Authorize(Roles = "admin")]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class StatisticsController: ControllerBase
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly IGoogleService _googleService;

    /// <summary>
    /// Constructor of StatisticsController
    /// </summary>
    /// <param name="calendarRepository"></param>
    /// <param name="googleService"></param>
    public StatisticsController(ICalendarRepository calendarRepository, IGoogleService googleService)
    {
        _calendarRepository = calendarRepository;
        _googleService = googleService;
    }

    /// <summary>
    /// Get statistics by event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<StatisticDto> GetStatisticByEvent(Guid eventId, CancellationToken ct = default)
    {
        var eventData = await _calendarRepository.GetByIdAsync(eventId, ct);
        var googleEventDetails = await _googleService.GetGoogleEventData(eventData.ExternalCalendarId, eventData.ExternalEventId, ct);

        return new StatisticDto
        {
            UserAttended = eventData.Users?.Count,
            QuestionCount = eventData.EventDetails?.Count,
            EventMinutesLength = (long)googleEventDetails.end.dateTime.Subtract(googleEventDetails.start.dateTime).TotalMinutes,
            IsGoogleMeet = googleEventDetails.conferenceData != null
        };
    }
}
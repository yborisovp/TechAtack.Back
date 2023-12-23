using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Filters;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace OggettoCase.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class CalendarController: ICalendarController
{
    private readonly ICalendarEventService _calendarEventService;
    private readonly ILogger<UserController> _logger;
    
    public CalendarController(ICalendarEventService calendarEventService, ILogger<UserController> logger)
    {
        _calendarEventService = calendarEventService;
        _logger = logger;
    }

    [HttpGet("filter")]
    public async Task<IEnumerable<CalendarDto>> GetFilteredAsync([FromQuery] CalendarFilter calendarFilter, CancellationToken ct = default)
    {
        var events = await _calendarEventService.GetFilteredEventsAsync(calendarFilter, ct);
        return events;
    }
    
    /// <inheritdoc />
    [HttpGet]
    [SwaggerOperation($"Get all {nameof(CalendarDto)}s")]
    [SwaggerResponse(200, type: typeof(IEnumerable<CalendarDto>), description: $"List of {nameof(CalendarDto)}s")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<IEnumerable<CalendarDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var events = await _calendarEventService.GetAllAsync(ct);
        return new OkObjectResult(events);
    }

    /// <inheritdoc />
    [HttpGet("{id:guid}")]
    [SwaggerOperation($"Get one {nameof(CalendarDto)}")]
    [SwaggerResponse(200, type: typeof(CalendarDto), description: $"Receive one {nameof(CalendarDto)} by id")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CalendarDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<CalendarDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var calendarEvent = await _calendarEventService.GetByIdAsync(id, ct);
        return calendarEvent;
    }
    
    /// <inheritdoc />
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ExcludeRoles")]
    [SwaggerOperation($"Update {nameof(CalendarDto)}")]
    [SwaggerResponse(200, type: typeof(CalendarDto), description: $"{nameof(CalendarDto)} successfully updated")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CalendarDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<CalendarDto>> UpdateByIdAsync(Guid id, [FromBody] UpdateCalendarDto dtoToUpdate, CancellationToken ct = default)
    {
        _logger.LogDebug("Update {name of} with id: '{id}''", nameof(CalendarDto), id);
        var updatedTemplate = await _calendarEventService.UpdateByIdAsync(id, dtoToUpdate, ct);
        _logger.LogDebug("Successfully update {CalendarDto} by id: '{id}'", nameof(CalendarDto), id);

        return updatedTemplate;
    }
    /// <inheritdoc />
    [HttpDelete("{id:Guid}")]
    [Authorize(Policy = "ExcludeRoles")]
    [SwaggerOperation($"Delete {nameof(CalendarDto)}")]
    [SwaggerResponse(200, type: typeof(CalendarDto), description: $"{nameof(CalendarDto)} successfully deleted")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CalendarDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<Guid>> DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Delete {name of} with id: '{id}'", nameof(CalendarDto), id);
        var deletedTemplateId = await _calendarEventService.DeleteByIdAsync(id, ct);
        _logger.LogDebug("Successfully delete {CalendarDto} by id: '{id}'", nameof(CalendarDto), id);

        return deletedTemplateId;
    }

    /// <inheritdoc />
    [Authorize(Policy = "ExcludeRoles")]
    [HttpPut("{id:Guid}/update-subscribers")]
    [SwaggerOperation($"Update {nameof(CalendarDto)}")]
    [SwaggerResponse(200, type: typeof(CalendarDto), description: $"{nameof(CalendarDto)} successfully updated")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CalendarDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<CalendarDto> UpdateSubscribedUsers(Guid eventId, [FromBody] IEnumerable<long> userIds, CancellationToken ct = default)
    {
        _logger.LogDebug("Update subscribers of calendar event with id: '{id}''", eventId);
        var updatedTemplate = await _calendarEventService.UpdateSubscribedUsersAsync(eventId, userIds, ct);
        _logger.LogDebug("Successfully subscribers of calendar event with id: '{id}'", nameof(CalendarDto), eventId);

        return updatedTemplate;
    }

    [Authorize(Roles = "admin,specialist")]
    [HttpPost("create")]
    [SwaggerOperation($"Create {nameof(CalendarDto)}")]
    [SwaggerResponse(200, type: typeof(CalendarDto), description: $"{nameof(CalendarDto)} successfully updated")]
    [SwaggerResponse(400, type: typeof(ValidationProblemDetails), description: "Validation error")]
    [SwaggerResponse(404, type: typeof(ProblemDetails), description: $"{nameof(CalendarDto)} with provided id doesn't exists")]
    [SwaggerResponse(500, type: typeof(ProblemDetails), description: "Server side error")]
    public async Task<ActionResult<CalendarDto>> CreateCalendarEvent([FromBody] CreateCalendarEventDto createCalendarEvent, CancellationToken ct = default)
    {
        var calendarEvent = await _calendarEventService.CreateCalendarEventAsync(createCalendarEvent, ct);
        return calendarEvent;
    }
}
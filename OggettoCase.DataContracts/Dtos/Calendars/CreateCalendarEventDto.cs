namespace OggettoCase.DataContracts.Dtos.Calendars;

public class CreateCalendarEventDto
{
    public required long OwnerId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTime StartedAt { get; set; }
    public required DateTime EndedAt { get; set; }
    public IList<long>? UserIds { get; set; }
    public string LinkToMeeting { get; set; } = string.Empty;
    
    public List<string>? AdditionalLinks { get; set; }
    public List<string>? EventDetails { get; set; }
}
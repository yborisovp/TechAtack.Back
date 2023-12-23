namespace OggettoCase.DataAccess.Dtos;

public class CreateCalendarEntityDto
{
    public required long OwnerId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTimeOffset StartedAt { get; set; }
    public required DateTimeOffset EndedAt { get; set; }
    public IList<long>? UserIds { get; set; }
    
    public required string LinkToMeeting { get; set; } 
}
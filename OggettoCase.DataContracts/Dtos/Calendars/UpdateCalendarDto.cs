using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Dtos.Calendars;

public class UpdateCalendarDto
{
    public Guid Id { get; set; }
    
    public required string? Title { get; set; }
    
    public DateTime? StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; } = DateTime.UtcNow;

    public string? Description { get; set; }
    
    public List<string>? AdditionalLinks { get; set; }
    public List<string>? EventDetails { get; set; }
}
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Dtos.Calendars;

public class UpdateCalendarDto
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? Description { get; set; }
    
    public UserDto Owner { get; set; }
}
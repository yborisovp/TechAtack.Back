using System.ComponentModel.DataAnnotations;
using OggettoCase.DataContracts.Dtos.Comments;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Dtos.Calendars;

public class CalendarDto
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public required string Title { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? Description { get; set; }
    
    public UserDto Owner { get; set; }
    
    public List<UserDto>? Users { get; set; }
    public List<CommentDto>? Comments { get; set; }
    public required string LinkToMeeting { get; set; } 

}
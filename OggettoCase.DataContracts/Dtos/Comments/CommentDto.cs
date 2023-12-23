using System.ComponentModel.DataAnnotations;
using OggettoCase.DataContracts.Dtos.Users;

namespace OggettoCase.DataContracts.Dtos.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    
    [MaxLength(355)]
    public required string Text { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public required Guid CalendarId { get; set; }
    
    public UserDto User { get; set; }
}
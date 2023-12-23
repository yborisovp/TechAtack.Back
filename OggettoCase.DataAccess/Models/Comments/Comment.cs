using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Models.Users;

namespace OggettoCase.DataAccess.Models.Comments;

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(355)]
    public required string Text { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("calendar_id")]
    public required Guid CalendarId { get; set; }

    public Calendar Calendar { get; set; } = null!;
    
    [Column("user_id")]
    public required long UserId { get; set; }
    public virtual User User { get; set; }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OggettoCase.DataAccess.Models.Comments;
using OggettoCase.DataAccess.Models.Users;

namespace OggettoCase.DataAccess.Models.Calendars;

public class Calendar
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [Column("started_at")]
    public DateTimeOffset StartedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [Column("ended_at")]
    public DateTimeOffset EndedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [ForeignKey(nameof(Owner))]
    [Column("owner_id")]
    public long OwnerId { get; set; }
    public User Owner { get; set; }
    
    
    public List<User>? Users { get; set; }
    
    public List<Comment>? Comments { get; set; }

    public string LinkToMeeting { get; set; } = string.Empty;
}
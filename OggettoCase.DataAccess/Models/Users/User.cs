using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Models.Categories;
using OggettoCase.DataAccess.Models.Users.Enums;

namespace OggettoCase.DataAccess.Models.Users;

public class User
{
    [Key]
    public required long Id { get; set; }
    
    [Column("external_id")]
    public string? ExternalId { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Surname { get; set; } = string.Empty;

    public UserRoleEnum Role { get; set; }

    [MaxLength(150)]
    public required string Email { get; set; }

    [Column("authentication_type")]
    public UserAuthenticationTypeEnum AuthenticationType { get; set; }
    
    [Column("access_token")]
    public string AccessToken { get; set; }
    
    [Column("is_approved")]
    public bool IsApproved { get; set; } = false;
    
    
    public Category? Category { get; set; }
    
    [Column("calendar_events")]
    public List<Calendar>? CalendarEvents { get; set; }
    
    [Column("photo_url")]
    public required string PhotoUrl { get; set; }

    [Column("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
    
    [Column("refresh_token_expiration_date")]
    public DateTime RefreshTokenExpirationDate { get; set; } = DateTime.MinValue;
}
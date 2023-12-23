namespace OggettoCase.Configuration;

public class JwtConfiguration
{
    
    /// <summary>
    /// Section is settings
    /// </summary>
    public const string OptionsKey = "JwtConfiguration";
    
    /// <summary>
    /// How is issuer
    /// </summary>
    public string Issuer { get; set; }
    /// <summary>
    /// Available addresess
    /// </summary>
    public string Audience { get; set; }
    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Authorization token life time
    /// </summary>
    public int JwtTokenExpirationTimeInMinutes { get; set; } = 2880;
    /// <summary>
    /// Refresh token lifetime
    /// </summary>
    public int RefreshTokenExpirationTimeInHours { get; set; } = 240;
    
    /// <summary>
    /// Class constructor
    /// </summary>
    public JwtConfiguration()
    {
        Issuer = string.Empty;
        Audience = string.Empty;
        Key = string.Empty;
    }
}
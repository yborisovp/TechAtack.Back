using System.Text.Json.Serialization;

namespace OggettoCase.DataContracts.Dtos.Users;

public class UserAuthorizationData
{
    [JsonPropertyName("id")]
    public string ExternalId { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("given_name")]
    public string Name { get; set; }
    
    [JsonPropertyName("family_name")]
    public string Surname { get; set; }
    
    [JsonPropertyName("picture")]
    public string PictureUrl { get; set; }
}
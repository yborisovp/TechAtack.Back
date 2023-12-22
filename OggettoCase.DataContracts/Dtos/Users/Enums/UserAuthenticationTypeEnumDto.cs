using System.Text.Json.Serialization;

namespace OggettoCase.DataContracts.Dtos.Users.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserAuthenticationTypeEnumDto
{
    Google
}
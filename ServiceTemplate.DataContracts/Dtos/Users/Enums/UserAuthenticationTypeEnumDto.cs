using System.Text.Json.Serialization;

namespace ServiceTemplate.DataContracts.Dtos.Users.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserAuthenticationTypeEnumDto
{
    Google
}
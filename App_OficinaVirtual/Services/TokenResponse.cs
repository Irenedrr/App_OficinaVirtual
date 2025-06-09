using System.Text.Json.Serialization;

namespace App_OficinaVirtual.Services;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}
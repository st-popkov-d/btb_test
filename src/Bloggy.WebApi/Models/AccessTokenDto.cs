using System.Text.Json.Serialization;

namespace Bloggy.WebApi.Models
{
    public class AccessTokenDto
    {
        [JsonPropertyName("accessToken")] public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("expiresIn")] public long ExpiresIn { get; set; } = -1;
    }
}

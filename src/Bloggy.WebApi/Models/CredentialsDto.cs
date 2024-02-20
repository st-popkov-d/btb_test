using System.Text.Json.Serialization;

namespace Bloggy.WebApi.Models
{
    public class CredentialsDto
    {
        [JsonPropertyName("username")] public string Username { get; set; }
        [JsonPropertyName("password")] public string Password { get; set; }
    }
}

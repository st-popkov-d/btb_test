using System.Text.Json.Serialization;

namespace Bloggy.WebApi.Models
{
    public class CreateCommentDto
    {
        [JsonIgnore] public Guid UserId { get; set; }
        [JsonIgnore] public Guid BlogPostId { get; set; }
        [JsonIgnore] public Guid CommentId { get; set; }
        [JsonPropertyName("content")] public string Content { get; set; }
    }
}

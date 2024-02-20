using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bloggy.WebApi.Models
{
    public class CreateBlogPostDto
    {
        [JsonIgnore] public Guid UserId { get; set; }
        [JsonIgnore] public Guid BlogPostId { get; set; }

        [Required]
        [MaxLength(127)]
        [JsonPropertyName("title")] 
        public string Title { get; set; }

        [Required]
        [MaxLength(16383)]
        [JsonPropertyName("content")] 
        public string Content { get; set; }
    }
}

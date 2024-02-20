namespace Bloggy.WebApi.Models
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<CommentDto> Comments { get; set; } = null;
    }
}

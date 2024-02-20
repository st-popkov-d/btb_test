namespace Bloggy.WebApi.Models
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

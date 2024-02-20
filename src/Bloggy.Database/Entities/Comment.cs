namespace Bloggy.Database.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}

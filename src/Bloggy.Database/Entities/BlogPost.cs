namespace Bloggy.Database.Entities
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}

using Bloggy.Database;
using Bloggy.Database.Entities;
using Bloggy.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.WebApi.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggyDbContext _context;

        public BlogPostRepository(BloggyDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddBlogAsync(CreateBlogPostDto createBlogPostDto)
        {
            var newBlogPost = new BlogPost
            {
                Content = createBlogPostDto.Content,
                Title = createBlogPostDto.Title,
                UserId = createBlogPostDto.UserId,
            };

            await _context.AddAsync(newBlogPost);
            await _context.SaveChangesAsync();
            return newBlogPost.Id;
        }

        public async Task<Guid> AddCommentAsync(CreateCommentDto createCommentDto)
        {
            var newComment = new Comment
            {
                Content = createCommentDto.Content,
                BlogPostId = createCommentDto.BlogPostId,
                UserId = createCommentDto.UserId
            };

            await _context.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return newComment.Id;
        }

        public async Task<bool> DeleteBlogAsync(Guid id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null) return false;
            _context.BlogPosts.Remove(blog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BlogPostDto> GetBlogAsync(Guid id)
        {
            var blog = await _context.BlogPosts
                .Where(x => x.Id == id)
                .Select(x => new BlogPostDto
                {
                    Id = x.Id,
                    Author = x.User.Username,
                    Content = x.Content,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    Comments = x.Comments.Select(x => new CommentDto { Id = x.Id, Content = x.Content, Author = x.User.Username, CreatedAt = x.CreatedAt }).ToList()
                }).FirstOrDefaultAsync();
            return blog;
        }

        public async Task<List<BlogPostDto>> GetBlogsAsync()
        {
            var blogs = await _context.BlogPosts
                .Select(x => new BlogPostDto
                {
                    Id = x.Id,
                    Author = x.User.Username,
                    Content = x.Content.Substring(0, 50),
                    Title = x.Title,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
            return blogs;
        }

        public async Task<bool> UpdateBlogAsync(CreateBlogPostDto updateBlogPostDto)
        {
            var blog = await _context.BlogPosts.FindAsync(updateBlogPostDto.BlogPostId);
            if (blog == null || blog.UserId != updateBlogPostDto.UserId) return false;

            blog.Title = updateBlogPostDto.Title;
            blog.Content = updateBlogPostDto.Content;

            _context.BlogPosts.Update(blog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCommentAsync(CreateCommentDto updateCommentDto)
        {
            var comment = await _context.Comments.FindAsync(updateCommentDto.CommentId);
            if (comment == null || comment.UserId != updateCommentDto.UserId) return false;

            comment.Content = updateCommentDto.Content;
            comment.UpdatedAt = DateTimeOffset.UtcNow;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using Bloggy.WebApi.Models;

namespace Bloggy.WebApi.Repository
{
    public interface IBlogPostRepository
    {
        Task<Guid> AddBlogAsync(CreateBlogPostDto createBlogPostDto);
        Task<Guid> AddCommentAsync(CreateCommentDto createCommentDto);
        Task<List<BlogPostDto>> GetBlogsAsync();
        Task<BlogPostDto> GetBlogAsync(Guid id);
        Task<bool> UpdateBlogAsync(CreateBlogPostDto updateBlogPostDto);
        Task<bool> UpdateCommentAsync(CreateCommentDto updateCommentDto);
        Task<bool> DeleteBlogAsync(Guid id);
        Task<bool> DeleteCommentAsync(Guid id);
    }
}

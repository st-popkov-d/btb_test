using Bloggy.WebApi.Models;
using Bloggy.WebApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.WebApi.Controllers
{
    [ApiController]
    [Route("blog")]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        [HttpPost("postBlog")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        public async Task<IActionResult> PostBlog([FromBody] CreateBlogPostDto blogPostDto)
        {
            blogPostDto.UserId = HttpContext.GetUserId();
            var blogId = await _blogPostRepository.AddBlogAsync(blogPostDto);
            return Ok(blogId);
        }

        [AllowAnonymous]
        [HttpGet("getBlogs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BlogPostDto>))]
        public async Task<IActionResult> GetBlogs()
        {
            var blogs = await _blogPostRepository.GetBlogsAsync();
            return Ok(blogs);
        }

        [AllowAnonymous]
        [HttpGet("getBlog/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogPostDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlog(Guid id)
        {
            var blog = await _blogPostRepository.GetBlogAsync(id);
            if (blog == null) return NotFound();
            return Ok(blog);
        }

        [HttpPut("updateBlog/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] CreateBlogPostDto blogPostDto)
        {
            blogPostDto.UserId = HttpContext.GetUserId();
            blogPostDto.BlogPostId = id;
            var updated = await _blogPostRepository.UpdateBlogAsync(blogPostDto);
            return updated ? Ok() : BadRequest();
        }

        [HttpDelete("deleteBlog/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var deleted =await _blogPostRepository.DeleteBlogAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("postComment/{blogId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        public async Task<IActionResult> PostComment(Guid blogId, [FromBody] CreateCommentDto commentDto)
        {
            commentDto.UserId = HttpContext.GetUserId();
            commentDto.BlogPostId = blogId;
            var commentId = await _blogPostRepository.AddCommentAsync(commentDto);
            return Ok(commentId);
        }

        [HttpPut("updateComment/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CreateCommentDto commentDto)
        {
            commentDto.UserId = HttpContext.GetUserId();
            commentDto.CommentId = id;
            var updated = await _blogPostRepository.UpdateCommentAsync(commentDto);
            return updated ? Ok() : BadRequest();
        }

        [HttpDelete("deleteComment/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var deleted = await _blogPostRepository.DeleteCommentAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}

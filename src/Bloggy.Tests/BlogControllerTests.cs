using Bloggy.Database.Entities;
using Bloggy.WebApi.Controllers;
using Bloggy.WebApi.Models;
using Bloggy.WebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bloggy.Tests
{
    public class BlogControllerTests
    {
        private Mock<IBlogPostRepository> _blogRepoMock;
        private BlogController _controller;

        private Guid _userId = Guid.Parse("5b090e6e-da9e-43d5-aa69-c318928a273e");

        public BlogControllerTests()
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, _userId.ToString()));
            var user = new ClaimsPrincipal(identity);
            var context = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            _blogRepoMock = new Mock<IBlogPostRepository>();
            _controller = new BlogController(_blogRepoMock.Object)
            {
                ControllerContext = context
            };
        }

        [Fact]
        public async Task PostBlog_Should_Return_Blog_Id()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.AddBlogAsync(It.Is<CreateBlogPostDto>(x => x.UserId == _userId))).ReturnsAsync(guid);

            var result = await _controller.PostBlog(new CreateBlogPostDto());

            var okObject = result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.Equal(guid, okObject.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(50)]
        public async Task GetBlogs_Should_Return_Collection_Of_Blogs(int count)
        {
            var blogs = Enumerable.Repeat(new BlogPostDto(), count).ToList();
            _blogRepoMock.Setup(x => x.GetBlogsAsync()).ReturnsAsync(blogs);

            var result = await _controller.GetBlogs();
            var okObject = result as OkObjectResult;

            Assert.NotNull(okObject);

            var model = okObject.Value as List<BlogPostDto>;
            Assert.NotNull(model);
            Assert.Equal(count, model.Count);
        }

        [Fact]
        public async Task GetBlog_Should_Return_Blog_If_Blog_Is_Found()
        {
            var guid = Guid.NewGuid();
            var blogPost = new BlogPostDto() { Id = guid };
            _blogRepoMock.Setup(x => x.GetBlogAsync(guid)).ReturnsAsync(blogPost);

            var result = await _controller.GetBlog(guid);
            var okObject = result as OkObjectResult;

            Assert.NotNull(okObject);
            Assert.Equal(blogPost, okObject.Value);
        }

        [Fact]
        public async Task GetBlog_Should_Return_404_If_Blog_Is_Not_Found()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.GetBlogAsync(guid)).ReturnsAsync(default(BlogPostDto));

            var result = await _controller.GetBlog(guid);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateBlog_Should_Return_200_If_Blog_Is_Updated()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.UpdateBlogAsync(It.Is<CreateBlogPostDto>(x => x.BlogPostId == guid))).ReturnsAsync(true);

            var result = await _controller.UpdateBlog(guid, new CreateBlogPostDto());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateBlog_Should_Return_400_If_Blog_Is_Not_Updated()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.UpdateBlogAsync(It.Is<CreateBlogPostDto>(x => x.BlogPostId == guid))).ReturnsAsync(false);

            var result = await _controller.UpdateBlog(guid, new CreateBlogPostDto());
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteBlog_Should_Return_204_If_Blog_Is_Deleted()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.DeleteBlogAsync(guid)).ReturnsAsync(true);

            var result = await _controller.DeleteBlog(guid);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBlog_Should_Return_404_If_Blog_Is_Not_Deleted()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.DeleteBlogAsync(guid)).ReturnsAsync(false);

            var result = await _controller.DeleteBlog(guid);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostComment_Should_Return_Comment_Id()
        {
            var blogId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.AddCommentAsync(It.Is<CreateCommentDto>(x => x.BlogPostId == blogId))).ReturnsAsync(commentId);

            var result = await _controller.PostComment(blogId, new CreateCommentDto());
            var okObject = result as OkObjectResult;

            Assert.NotNull(okObject);
            Assert.Equal(commentId, okObject.Value);
        }

        [Fact]
        public async Task UpdateComment_Should_Return_200_If_Comment_Is_Updated()
        {
            var commentId = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.UpdateCommentAsync(It.Is<CreateCommentDto>(x => x.CommentId == commentId))).ReturnsAsync(true);

            var result = await _controller.UpdateComment(commentId, new CreateCommentDto());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateComment_Should_Return_400_If_Comment_Is_Not_Updated()
        {
            var commentId = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.UpdateCommentAsync(It.Is<CreateCommentDto>(x => x.CommentId == commentId))).ReturnsAsync(false);

            var result = await _controller.UpdateComment(commentId, new CreateCommentDto());
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteComment_Should_Return_204_If_Comment_Is_Deleted()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.DeleteCommentAsync(guid)).ReturnsAsync(true);

            var result = await _controller.DeleteComment(guid);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteComment_Should_Return_404_If_Comment_Is_Not_Deleted()
        {
            var guid = Guid.NewGuid();
            _blogRepoMock.Setup(x => x.DeleteCommentAsync(guid)).ReturnsAsync(false);

            var result = await _controller.DeleteComment(guid);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

using Bloggy.WebApi.Controllers;
using Bloggy.WebApi.Models;
using Bloggy.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggy.Tests
{
    public class AuthControllerTests
    {
        private readonly CredentialsDto _credentials = new CredentialsDto
        {
            Username = "test",
            Password = "test"
        };

        private readonly AccessTokenDto _accessToken = new AccessTokenDto
        {
            AccessToken = "token",
            ExpiresIn = 30
        };


        [Fact]
        public async Task Login_Should_Return_Access_Token_When_Auth_Is_Successful()
        {

            var authMock = new Mock<IAuthService>();
            authMock.Setup(x => x.GetAccessToken(_credentials)).ReturnsAsync(_accessToken);

            var controller = new AuthController(authMock.Object);

            var result = await controller.Login(_credentials);
            var objectResult = result as ObjectResult;

            Assert.IsType<OkObjectResult>(objectResult);
            Assert.Equal(_accessToken, objectResult.Value);
        }

        [Fact]
        public async Task Login_Should_Return_400_When_Auth_Is_Not_Successfull()
        {
            var authMock = new Mock<IAuthService>();
            authMock.Setup(x => x.GetAccessToken(It.IsAny<CredentialsDto>())).ReturnsAsync(default(AccessTokenDto));

            var controller = new AuthController(authMock.Object);

            var result = await controller.Login(_credentials);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}

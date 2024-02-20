using Bloggy.WebApi;
using Bloggy.WebApi.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Bloggy.Tests.AuthenticationTest
{
    public class TestWebApplication : WebApplicationFactory<Program>
    {
        public Mock<IBlogPostRepository> _blogPostRepository = new Mock<IBlogPostRepository>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            Environment.SetEnvironmentVariable("ConnectionStrings__Bloggy", "unset");
            Environment.SetEnvironmentVariable("Jwt__Secret", "secretly-secret-test-key-secretly-secret-test-key");
            Environment.SetEnvironmentVariable("Jwt__Audience", "test");
            Environment.SetEnvironmentVariable("Jwt__Issuer", "test");
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IBlogPostRepository>(_blogPostRepository.Object);
            });
            builder.UseEnvironment("Testing");
        }
    }

    /*
     * Since authentication is implemented via a combination of Auth Filter & Attribute 
     * we can't test it creating a controller object, since short-circuit happend before
     * our code is reached. In order to test authentication, WebApplicationFactory can be used,
     * which creates configurable copy of web api.
     */

    public class AuthenticationTest
    {

        [Fact]
        public async Task Application_Should_Return_401_If_Jwt_Token_Is_Missing()
        {
            await using var application = new TestWebApplication();
            var client = application.CreateClient();

            var response = await client.DeleteAsync($"/blog/deleteBlog/{Guid.NewGuid()}");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Application_Should_Return_401_If_Jwt_Token_Is_Malformed()
        {
            await using var application = new TestWebApplication();
            var client = application.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer malformed");

            var response = await client.DeleteAsync($"/blog/deleteBlog/{Guid.NewGuid()}");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Application_Should_Not_Return_401_If_Jwt_Token_Is_Valid()
        {
            await using var application = new TestWebApplication();
            var client = application.CreateClient();
            var token = CreateToken();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.DeleteAsync($"/blog/deleteBlog/{Guid.NewGuid()}");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        private string CreateToken()
        {
            var secret = "secretly-secret-test-key-secretly-secret-test-key";
            var issuer = "test";
            var audience = "test";
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            };

            var expires = DateTimeOffset.UtcNow.AddHours(3);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires.UtcDateTime,
                signingCredentials: signinCredentials);

            var securityTokenHandler = new JwtSecurityTokenHandler();
            var token = securityTokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}

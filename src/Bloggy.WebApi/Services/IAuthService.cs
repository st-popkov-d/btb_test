using Bloggy.WebApi.Models;

namespace Bloggy.WebApi.Services
{
    public interface IAuthService
    {
        Task<AccessTokenDto> GetAccessToken(CredentialsDto credentialsDto);
    }
}

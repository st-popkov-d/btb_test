using System.IdentityModel.Tokens.Jwt;

namespace Bloggy.WebApi
{
    public static class ExtensionMethods
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            var claimValue = httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            if (!string.IsNullOrWhiteSpace(claimValue)) return Guid.Parse(claimValue);
            return Guid.Empty;
        }
    }
}

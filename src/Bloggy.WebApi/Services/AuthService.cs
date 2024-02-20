using Bloggy.Database;
using Bloggy.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bloggy.WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly BloggyDbContext _context;
        private readonly ITimeProvider _timeProvider;
        private readonly IConfiguration _configuration;

        public AuthService(BloggyDbContext context, ITimeProvider timeProvider, IConfiguration configuration)
        {
            _context = context;
            _timeProvider = timeProvider;
            _configuration = configuration;
        }

        public async Task<AccessTokenDto> GetAccessToken(CredentialsDto credentialsDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == credentialsDto.Username);
            if (user == null) return null;

            var calculatedHash = DummyHashFn(credentialsDto.Password, user.PasswordSalt);
            if (calculatedHash != user.PasswordHash)
            {
                return null;
            }

            var secret = _configuration.GetValue<string>("Jwt:Secret");
            var issuer = _configuration.GetValue<string>("Jwt:Issuer");
            var audience = _configuration.GetValue<string>("Jwt:Audience");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var expires = _timeProvider.UtcNow.AddHours(1);
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
            return new AccessTokenDto
            {
                AccessToken = token,
                ExpiresIn = (long)expires.Subtract(_timeProvider.UtcNow).TotalSeconds
            };
        }

        /*
         * This method is placed here only for convenience,
         * and is a copy of Bloggy.Database.BloggyDbContext.DummyHashFn.
         * Ideally it should be extracted and placed in Libs/Utils project of solution.
         */
        private string DummyHashFn(string input, string salt)
        {
            var saltBytes = Convert.FromHexString(salt);
            var passwordBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: passwordBytes,
                salt: saltBytes,
                iterations: 5,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 20);
            var hash = Convert.ToHexString(hashBytes);
            return hash;
        }
    }
}

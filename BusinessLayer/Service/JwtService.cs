using System.Security.Claims;
using System.Text;
using BusinessLayer.Constants;
using BusinessLayer.Interface;
using BusinessLayer.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;

namespace BusinessLayer.Service
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public (string Token, DateTime ExpiresAt) GenerateToken(UserEntity user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimNames.UserId, user.UserId.ToString()),
                    new Claim(ClaimNames.Email, user.Email)
                }),
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return (token, expiresAt);
        }
    }
}

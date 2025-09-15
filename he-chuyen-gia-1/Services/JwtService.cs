using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hechuyengia.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace hechuyengia.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _cfg;
        public JwtService(IConfiguration cfg) => _cfg = cfg;

        public string CreateToken(User u)
        {
            // lấy ExpireMinutes từ config (mặc định 120)
            var expireMinutes = int.TryParse(_cfg["Jwt:ExpireMinutes"], out var m) ? m : 120;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.UserId.ToString()),
                new Claim(ClaimTypes.Role, u.Role),
                new Claim("fullName", u.FullName ?? string.Empty),
                new Claim("email", u.Email)
            };

            var secret = _cfg["Jwt:Secret"]
                         ?? throw new InvalidOperationException("Missing Jwt:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

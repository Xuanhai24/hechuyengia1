using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hechuyengia.Auth;
using hechuyengia.Models;
using Microsoft.IdentityModel.Tokens;

namespace hechuyengia.Auth
{
    public static class JwtTokenFactory
    {
        public static string CreateToken(User user, JwtOptions opt)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: opt.Issuer,
                audience: opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(opt.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

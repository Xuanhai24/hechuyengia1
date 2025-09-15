using System.Text;
using hechuyengia.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace hechuyengia.Extensions
{
    public static class AuthSetupExtensions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            // Bind JwtOptions từ appsettings.json
            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            var jwt = config.GetSection("Jwt").Get<JwtOptions>()
                      ?? throw new InvalidOperationException("Missing 'Jwt' section in appsettings.json");

            if (string.IsNullOrWhiteSpace(jwt.Secret))
                throw new InvalidOperationException("Missing Jwt:Secret in appsettings.json");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(o =>
                    {
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwt.Issuer,
                            ValidAudience = jwt.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
                        };
                    });

            services.AddAuthorization();
            return services;
        }
    }
}

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TestMe.Presentation.API.Services;

namespace TestMe.Presentation.API.Configurations
{
    internal static class JWTAuthentication
    {
        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = config[AuthenticationService.ConfigurationIssuer],
                            ValidAudience = config[AuthenticationService.ConfigurationIssuer],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[AuthenticationService.ConfigurationKey]))
                        };
                    });
        }
    }
}

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestMe.Presentation.API.Services;

namespace TestMe.Presentation.API.Configurations
{
    internal static class JWTAuthentication
    {
        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration, string sectionKey)
        {
            var config = new AuthenticationService.Config();
            configuration.Bind(sectionKey, config);

            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = config.Issuer,
                            ValidAudience = config.Issuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key))
                        };
                    });
        }
    }
}

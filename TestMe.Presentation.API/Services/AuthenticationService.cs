using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TestMe.UserManagement.App.Users.Output;

namespace TestMe.Presentation.API.Services
{
    public sealed class AuthenticationService
    {
        public static string BuildToken(UserCredentialsDTO userCredentials, Config config)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userCredentials.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userCredentials.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                  claims: claims,
                  issuer:  config.Issuer,
                  audience: config.Issuer,
                  expires: DateTime.Now.AddMonths(1),
                  signingCredentials: credentials
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public class Config
        {
            public string Issuer { get; set; }
            public string Key { get; set; }

            public Config()
            {
                Issuer = String.Empty;
                Key = String.Empty;
            }
        }
    }
}

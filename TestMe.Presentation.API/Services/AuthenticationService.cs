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
        public const string ConfigurationIssuer = "Jwt:Issuer";
        public const string ConfigurationKey = "Jwt:Key";


        public static string BuildToken(UserCredentialsDTO userCredentials, string jwtIssuer, string jwtKey)
        {
            var claims = new Claim[]
            {              
                new Claim(ClaimTypes.NameIdentifier, userCredentials.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userCredentials.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                  claims: claims,
                  issuer: jwtIssuer,
                  audience: jwtIssuer,
                  expires: DateTime.Now.AddMonths(1),
                  signingCredentials: credentials
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

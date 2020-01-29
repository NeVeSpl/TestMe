using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestMe.Presentation.API.Controllers.Tokens.Input;
using TestMe.Presentation.API.Controllers.Tokens.Output;
using TestMe.Presentation.API.Services;
using TestMe.UserManagement.App.Users;


namespace TestMe.Presentation.API.Controllers.Tokens
{
    [Route("[controller]")]    
    public class TokensController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateToken(LoginCredentialsDTO loginCredentials, [FromServices]UsersService service, [FromServices]IConfiguration config)
        {
            var userCredentials = service.VerifyUserCredentials(loginCredentials.CreateCommand());
            IActionResult response;
            if (userCredentials != null)
            {
                var tokenString = AuthenticationService.BuildToken(userCredentials, config[AuthenticationService.ConfigurationIssuer], config[AuthenticationService.ConfigurationKey]);
                response = Ok(new TokenDTO { Token = tokenString });
            }
            else
            {
                response = Unauthorized();
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost("Async")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        public async Task<IActionResult> CreateTokenAsync(LoginCredentialsDTO loginCredentials, [FromServices]UsersService service, [FromServices]IConfiguration config)
        {
            var userCredentials = await service.VerifyUserCredentialsAsync(loginCredentials.CreateCommand());
            IActionResult response;
            if (userCredentials != null)
            {
                var tokenString = AuthenticationService.BuildToken(userCredentials, config[AuthenticationService.ConfigurationIssuer], config[AuthenticationService.ConfigurationKey]);
                response = Ok(new TokenDTO { Token = tokenString });
            }
            else
            {
                response = Unauthorized();
            }

            return response;
        }       
    }
}
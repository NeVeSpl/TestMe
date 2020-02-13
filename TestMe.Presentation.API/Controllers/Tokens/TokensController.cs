using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        public ActionResult<TokenDTO> CreateToken(LoginCredentialsDTO loginCredentials, [FromServices]UsersService service, [FromServices]IOptions<AuthenticationService.Config> config)
        {
            var result = service.VerifyUserCredentials(loginCredentials.CreateCommand());
            ActionResult<TokenDTO> response;
            if (result.IsAuthenticated)
            {
                var tokenString = AuthenticationService.BuildToken(result.UserCredentials!, config.Value);
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
        public async Task<ActionResult<TokenDTO>> CreateTokenAsync(LoginCredentialsDTO loginCredentials, [FromServices]UsersService service, [FromServices]IOptions<AuthenticationService.Config> config)
        {
            var result = await service.VerifyUserCredentialsAsync(loginCredentials.CreateCommand());
            ActionResult<TokenDTO> response;
            if (result.IsAuthenticated)
            {
                var tokenString = AuthenticationService.BuildToken(result.UserCredentials!, config.Value);
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
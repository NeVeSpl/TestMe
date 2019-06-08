using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestMe.Presentation.API.Services;
using TestMe.UserManagement.App.Users;
using TestMe.UserManagement.App.Users.DTO;

namespace TestMe.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class TokensController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateToken(LoginCredentials loginCredentials, [FromServices]UsersService service, [FromServices]IConfiguration config)
        {
            IActionResult response = null;

            var user = service.GetAuthenticatedUser(loginCredentials);
            if (user != null)
            {
                var tokenString = AuthenticationService.BuildToken(user, config[AuthenticationService.ConfigurationIssuer], config[AuthenticationService.ConfigurationKey]);
                response = Ok(new { token = tokenString });
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
        public async Task<IActionResult> CreateTokenAsync(LoginCredentials loginCredentials, [FromServices]UsersService service, [FromServices]IConfiguration config)
        {
            IActionResult response = null;

            var user = await service.GetAuthenticatedUserAsync(loginCredentials);
            if (user != null)
            {
                var tokenString = AuthenticationService.BuildToken(user, config[AuthenticationService.ConfigurationIssuer], config[AuthenticationService.ConfigurationKey]);
                response = Ok(new { token = tokenString });
            }
            else
            {
                response = Unauthorized();
            }

            return response;
        }       
    }
}
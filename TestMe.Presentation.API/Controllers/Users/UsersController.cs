using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Controllers.Users.Input;
using TestMe.UserManagement.App.Users;
using TestMe.UserManagement.App.Users.Input;
using TestMe.UserManagement.App.Users.Output;
using TestMe.UserManagement.Domain;

namespace TestMe.Presentation.API.Controllers.Users
{
    [Route("[controller]")]
    [ApiConventionType(typeof(ApiConventions))]
    public class UsersController : Controller
    {
        private readonly UsersService service;


        public UsersController(UsersService service)
        {
            this.service = service;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<long>> CreateUser(CreateUserDTO createUser)
        {
            var result = await service.CreateUser(createUser.CreateCommand());
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CursorPagedResults<UserDTO>>> ReadUsers([FromQuery]CursorPagination pagination)
        {
            var result = await service.GetUsers(new ReadUsers() { Pagination = pagination });
            return Ok(result);
        }

        [HttpGet("EmailAddress/IsTaken")]
        public async Task<ActionResult<bool>> IsEmailAddressTaken(string emailAddress)
        {
            var result = await service.IsEmailAddressTaken(emailAddress);
            return Ok(result);
        }
    }
}
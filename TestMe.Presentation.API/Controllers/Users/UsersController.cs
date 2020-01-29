using System.ComponentModel.DataAnnotations;
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
        public ActionResult<long> CreateUser(CreateUserDTO createUser)
        {
            var result = service.CreateUser(createUser.CreateCommand());
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<CursorPagedResults<UserDTO>> ReadUsers(int cursor = 0, [Range(-10, 10)]int fetchNext = 10)
        {
            var result = service.GetUsers(new ReadUsers() { Cursor = cursor, FetchNext = fetchNext });
            return Ok(result);
        }
    }
}
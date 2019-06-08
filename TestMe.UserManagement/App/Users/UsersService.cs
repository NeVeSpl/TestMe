using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestMe.UserManagement.App.Users.DTO;
using TestMe.UserManagement.Domain;
using TestMe.UserManagement.Persistence;

namespace TestMe.UserManagement.App.Users
{
    public class UsersService
    {
        private readonly UserManagementDbContext context;


        public UsersService(UserManagementDbContext context)
        {
            this.context = context;
        }


        public User GetAuthenticatedUser(LoginCredentials credentials)
        {
            return context.Users.AsNoTracking().FirstOrDefault(x => x.Name == credentials.Login);
        }

        public async Task<User> GetAuthenticatedUserAsync(LoginCredentials credentials)
        {
            return await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == credentials.Login);
        }

        public async ValueTask<User> GetAuthenticatedUserAsyncVT(LoginCredentials credentials)
        {
            return await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == credentials.Login);
        }
    }
}

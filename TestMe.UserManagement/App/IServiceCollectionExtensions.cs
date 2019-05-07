using TestMe.UserManagement.App.Users;
using Microsoft.Extensions.DependencyInjection;

namespace TestMe.UserManagement.App
{
    public static class IServiceCollectionExtensions
    {
        public static void AddUserManagementApplicationServices(this IServiceCollection services)
        {  
            services.AddTransient<UsersService, UsersService>();
        }
    }
}

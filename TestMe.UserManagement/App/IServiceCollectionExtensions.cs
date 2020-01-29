using Microsoft.Extensions.DependencyInjection;
using TestMe.UserManagement.App.Users;

namespace TestMe.UserManagement.App
{
    public static class IServiceCollectionExtensions
    {
        public static void AddUserManagementApplicationServices(this IServiceCollection services)
        {  
            services.AddTransient<UsersService>();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestMe.UserManagement.Persistence
{
    public static class IServiceCollectionExtensions
    {
        public static void AddUserManagementPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<UserManagementDbContext>(options => options.UseNpgsql(connectionString));           
        }
    }
}

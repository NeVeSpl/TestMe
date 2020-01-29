using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestMe.UserManagement.Persistence.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static void MigrateUserManagementDb(this IServiceProvider applicationServices)
        {
            using (var scope = applicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<UserManagementDbContext>().Database.Migrate();              
            }
        }
    }
}

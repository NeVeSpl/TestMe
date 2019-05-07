using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestMe.UserManagement.Persistence;

namespace TestMe.TestCreation.Persistence
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

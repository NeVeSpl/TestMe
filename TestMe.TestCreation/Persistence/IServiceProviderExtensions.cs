using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestMe.TestCreation.Persistence
{
    public static class IServiceProviderExtensions
    {
        public static void MigrateTestCreationDb(this IServiceProvider applicationServices)
        {
            using (var scope = applicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<TestCreationDbContext>().Database.Migrate();              
            }
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestMe.TestCreation.Persistence
{
    public static class IServiceProviderExtensionsFromTestCreationPersistence
    {
        public static void MigrateTestCreationDb(this IServiceProvider applicationServices)
        {
            using (var scope = applicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<TestCreationDbContext>().Database.Migrate();              
            }
        }
    }
}

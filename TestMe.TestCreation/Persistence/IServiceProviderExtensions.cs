using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.Persistence;

namespace System
{
    public static class IServiceProviderExtensionsFromTestCreationPersistence
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

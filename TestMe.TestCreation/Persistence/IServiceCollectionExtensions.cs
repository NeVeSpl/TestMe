using Microsoft.EntityFrameworkCore;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensionsFromTestCreationPersistence
    {
        public static void AddTestCreationPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<TestCreationDbContext>(options => { options.UseNpgsql(connectionString); options.EnableSensitiveDataLogging(); });
            services.AddDbContextPool<ReadOnlyTestCreationDbContext>(options => 
            {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging();
            });
            services.AddTransient<ITestCreationUoW, TestCreationUoW>();
        }
    }
}

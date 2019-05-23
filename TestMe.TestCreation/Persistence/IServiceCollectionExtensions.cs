using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    public static class IServiceCollectionExtensions
    {
        public static void AddTestCreationPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<TestCreationDbContext>(options => { options.UseNpgsql(connectionString); options.EnableSensitiveDataLogging(); });
            services.AddDbContextPool<ReadOnlyTestCreationDbContext>(options => { options.UseNpgsql(connectionString); options.EnableSensitiveDataLogging(); });
            services.AddTransient<ITestCreationUoW, TestCreationUoW>();
        }
    }
}

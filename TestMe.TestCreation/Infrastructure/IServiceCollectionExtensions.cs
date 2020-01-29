using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.Infrastructure.EventBus;

namespace TestMe.TestCreation.Infrastructure
{
    public static class IServiceCollectionExtensionsFromTestCreationApp
    {
        public static void AddTestCreationInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<EventReceiver>();
        }
    }
}
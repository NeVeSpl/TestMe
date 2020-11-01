using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.App.EventHandlers.UserManagementEvents;

namespace TestMe.TestCreation.App
{
    public static class IServiceCollectionExtensionsFromTestCreationApp
    {
        public static void AddTestCreationApplicationServices(this IServiceCollection services)
        { 
            services.AddTransient<UserCreatedEventHandler>();         
        }
    }
}
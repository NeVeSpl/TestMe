using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TestMe.BuildingBlocks.EventBus;
using TestMe.TestCreation.App.EventHandlers;
using TestMe.TestCreation.Infrastructure.EventBus;
using TestMe.UserManagement.IntegrationEvents;

namespace TestMe.TestCreation.App
{
    public static class IApplicationBuilderExtensionsFromTestCreationApp
    {
        public static void SubscribeTestCreationEventHandlers(this IApplicationBuilder applicationBuilder)
        {
            var eventBus = applicationBuilder.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UserCreatedV1, UserCreatedEventHandler, EventReceiver>();           
        }
    }
}

using TestMe.BuildingBlocks.EventBus;
using TestMe.TestCreation.App.EventHandlers.UserManagementEvents;
using TestMe.TestCreation.Infrastructure.EventBus;
using TestMe.UserManagement.IntegrationEvents;

namespace TestMe.TestCreation.App
{
    public static class IEventBusExtensionsFromTestCreationApp
    {
        public static void SubscribeTestCreationEventHandlers(this IEventBus eventBus)
        {            
            eventBus.Subscribe<UserCreatedV1, UserCreatedEventHandler, EventReceiver>("TestMe.TestCreation.App");           
        }
    }
}

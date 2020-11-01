using System;
using System.Threading.Tasks;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Infrastructure.EventBus.InMemory
{
    public partial class InMemoryEventBus : IEventBus
    {
        private readonly IServiceProvider services;
        private readonly SubscriptionsManager subscriptionsManager = new SubscriptionsManager();

        public InMemoryEventBus(IServiceProvider services)
        {
            this.services = services;
        }


        public async Task<bool> Publish(Event @event)
        {
            foreach (var subscription in subscriptionsManager.GetSubscriptions(@event.RoutingKey))
            {
                await subscription.ProcessEvent(services, @event);                
            }            
            
            return true;                        
        }

        public void Subscribe<TEvent, TEventHandler, TEventInterceptor>(string queueName)
            where TEvent : class
            where TEventHandler : IEventHandler<TEvent>
            where TEventInterceptor : IEventInterceptor
        {
            subscriptionsManager.Subscribe<TEvent, TEventHandler, TEventInterceptor>(queueName);    
        }
    }
}
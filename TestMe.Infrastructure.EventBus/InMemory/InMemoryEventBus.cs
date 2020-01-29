using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Infrastructure.EventBus.InMemory
{
    public partial class InMemoryEventBus : IEventBus
    {
        private readonly IServiceProvider services;
        private readonly Dictionary<string, SubscriptionInfo> eventHandlers = new Dictionary<string, SubscriptionInfo>();

        public InMemoryEventBus(IServiceProvider services)
        {
            this.services = services;
        }


        public async Task<bool> Publish(Event @event)
        {
            if (eventHandlers.ContainsKey(@event.RoutingKey))
            {
                using (var scope = services.CreateScope())
                {
                    var subscription = eventHandlers[@event.RoutingKey];
                    var eventHandler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);

                    if (eventHandler == null)
                    {
                        return true;
                    }

                    Type eventHandlerType = typeof(IEventHandler<>).MakeGenericType(subscription.PayloadType);
                    MethodInfo handler = eventHandlerType.GetMethod(nameof(IEventHandler<object>.Handle))!;
                    var payload = JsonSerializer.Deserialize(@event.Payload, subscription.PayloadType);

                    await Task.Yield();

                    if (scope.ServiceProvider.GetRequiredService(subscription.InterceptorType) is IEventInterceptor eventInterceptor)
                    {
                        await eventInterceptor.ReceiveEvent(@event, () => (Task)handler.Invoke(eventHandler, new[] { payload })!);
                    }
                    else
                    {
                        await (Task)handler.Invoke(eventHandler, new[] { payload })!;
                    }
                }
            }            
            
            return true;                        
        }

        public void Subscribe<T, TEH, TEI>()
            where T : class
            where TEH : IEventHandler<T>
            where TEI : IEventInterceptor
        {
            var routingKey = GetEventKey<T>();
            eventHandlers[routingKey] = new SubscriptionInfo(typeof(T), typeof(TEH), typeof(TEI));
        }

        private string GetEventKey<T>()
        {
#pragma warning disable CS8603 // Possible null reference return.
            return typeof(T).FullName;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
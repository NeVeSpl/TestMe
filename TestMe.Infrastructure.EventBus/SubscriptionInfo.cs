using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Infrastructure.EventBus
{   
    internal class SubscriptionInfo
    {         
        public Type PayloadType { get; }
        public Type HandlerType { get; }
        public Type InterceptorType { get; }

        public SubscriptionInfo(Type payloadType, Type handlerType, Type interceptorType)
        {
            PayloadType = payloadType;
            HandlerType = handlerType;
            InterceptorType = interceptorType;
        }

        public async Task ProcessEvent(IServiceProvider services, Event @event)
        {
            using (var scope = services.CreateScope())
            {
                var eventHandler = scope.ServiceProvider.GetRequiredService(HandlerType);

                if (eventHandler == null)
                {
                    return;
                }

                Type eventHandlerType = typeof(IEventHandler<>).MakeGenericType(PayloadType);
                MethodInfo handler = eventHandlerType.GetMethod(nameof(IEventHandler<object>.Handle))!;
                var payload = JsonSerializer.Deserialize(@event.Payload, PayloadType);

                if (scope.ServiceProvider.GetRequiredService(InterceptorType) is IEventInterceptor eventInterceptor)
                {
                    await eventInterceptor.ReceiveEvent(@event, () => (Task)handler.Invoke(eventHandler, new[] { payload })!);
                }
                else
                {
                    await (Task)handler.Invoke(eventHandler, new[] { payload })!;
                }
            }
        }
    }    
}
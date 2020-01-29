using System;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Infrastructure.EventBus.InMemory
{
    public partial class InMemoryEventBus : IEventBus
    {
        private class SubscriptionInfo
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
        }
    }
}
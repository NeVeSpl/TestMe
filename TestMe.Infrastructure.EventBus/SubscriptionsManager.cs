using System;
using System.Collections.Generic;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Infrastructure.EventBus
{
    internal sealed class SubscriptionsManager
    {
        private readonly Dictionary<string, Dictionary<string, SubscriptionInfo>> queues = new Dictionary<string, Dictionary<string, SubscriptionInfo>>();


        public IEnumerable<SubscriptionInfo> GetSubscriptions(string routingKey)
        {
            foreach(var keyPair in queues)
            {
                if (keyPair.Value.ContainsKey(routingKey))
                {
                    yield return keyPair.Value[routingKey];
                }
            }
        }

        public SubscriptionInfo? GetSubscription(string queueName, string routingKey)
        {
            if (queues.ContainsKey(queueName))
            {
                if (queues[queueName].ContainsKey(routingKey))
                {
                    return queues[queueName][routingKey];
                }
            }
            return null;
        }
        
        public void Subscribe<TEvent, TEventHandler, TEventInterceptor>(string queueName)
           where TEvent : class
           where TEventHandler : IEventHandler<TEvent>
           where TEventInterceptor : IEventInterceptor
        {
            if (!queues.ContainsKey(queueName))
            {
                queues[queueName] = new Dictionary<string, SubscriptionInfo>();
            }

            var routingKey = Event.GetRoutingKey<TEvent>();

            if (queues[queueName].ContainsKey(routingKey))
            {
                throw new Exception($"Queue: {queueName} already has registered event handler for event : {routingKey}");
            }

            queues[queueName][routingKey] = new SubscriptionInfo(typeof(TEvent), typeof(TEventHandler), typeof(TEventInterceptor));
        }
    }
}

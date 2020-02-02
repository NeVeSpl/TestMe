using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Infrastructure.EventBus.RabbitMQ
{
    public sealed partial class RabbitMQEventBus : IEventBus
    {
        private static readonly string  ExchangeName = "TestMe.IntegrationEvents";     
        private readonly IServiceProvider services;
        private readonly RabbitMQPersistentConnection persistentConnection;
        private readonly SubscriptionsManager subscriptionsManager = new SubscriptionsManager();  
        private readonly Dictionary<string, IModel> openedChannels = new Dictionary<string, IModel>();

        public RabbitMQEventBus(IServiceProvider services, IOptions<Config> config)
        {           
            this.services = services;
            persistentConnection = new RabbitMQPersistentConnection(config.Value.HostName);
        }

        public Task<bool> Publish(Event @event)
        {           
            using (var channel = persistentConnection.CreateChannel())
            {
                channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true, autoDelete : false);
                channel.ConfirmSelect();

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                var message = JsonSerializer.Serialize(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: ExchangeName, routingKey: @event.RoutingKey, basicProperties: properties, body: body);
                channel.WaitForConfirms(new TimeSpan(0, 0, 5));
            }
            return Task.FromResult<bool>(true);
        }        

        public void Subscribe<TEvent, TEventHandler, TEventInterceptor>(string queueName)
            where TEvent : class
            where TEventHandler : IEventHandler<TEvent>
            where TEventInterceptor : IEventInterceptor
        {
            subscriptionsManager.Subscribe<TEvent, TEventHandler, TEventInterceptor>(queueName);            
            var routingKey = Event.GetRoutingKey<TEvent>();

            bool isConsumerCreatedForQueue = openedChannels.ContainsKey(queueName);

            using (var channel = persistentConnection.CreateChannel())
            {
                if (!isConsumerCreatedForQueue)
                {
                    channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true, autoDelete: false);
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);
                }
                channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: routingKey);
            }

            if (!isConsumerCreatedForQueue)
            {
                CreateConsumerForQueue(queueName);                
            }           
        }

        private void CreateConsumerForQueue(string queueName)
        {
            if ((openedChannels.ContainsKey(queueName) && openedChannels[queueName]!= null))
            {
                openedChannels[queueName].CallbackException -= CallbackException;
                openedChannels[queueName].Dispose();
            }
            openedChannels[queueName] = persistentConnection.CreateChannel();
            openedChannels[queueName].CallbackException += CallbackException;            
            var consumer = new AsyncEventingBasicConsumer(openedChannels[queueName]);
            consumer.Received += ConsumerReceived;
            openedChannels[queueName].BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            void CallbackException(object? sender, CallbackExceptionEventArgs e)
            {
                CreateConsumerForQueue(queueName);
            }
            async Task ConsumerReceived(object? sender, BasicDeliverEventArgs eventArgs)
            {
                //await Task.Yield(); https://github.com/NickLydon/RabbitMQAsyncDeadlock
                try
                {
                    var body = eventArgs.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = eventArgs.RoutingKey;

                    Event @event = JsonSerializer.Deserialize<Event>(message);
                    var subscription = subscriptionsManager.GetSubscription(queueName, routingKey);
                    if (subscription != null)
                    {
                        await subscription.ProcessEvent(services, @event);
                    }

                    openedChannels[queueName].BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    //todo
                    // https://www.rabbitmq.com/dlx.html
                    //channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, true);
                }
            }
        }        
    }
}
using System;
using System.Text.Json;

namespace TestMe.BuildingBlocks.EventBus
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string RoutingKey { get; set; }
        public string CorrelationId { get; set; }
        public string Payload { get; set; }
        public DateTimeOffset? PostDateTime { get; set;}
        public DateTimeOffset? ReceivedDateTime { get; set; }


#pragma warning disable CS8618 // Non-nullable field is uninitialized. Constructor neeed by EF core
        public Event()
        {

        }
#pragma warning restore CS8618

        public Event(object message, string correlationId)
        {           
            Payload = JsonSerializer.Serialize(message);
            RoutingKey = GetRoutingKey(message.GetType());
            CorrelationId = correlationId;
        }


        public static string GetRoutingKey(Type type)
        {
            return type.FullName ?? String.Empty;
        }
        public static string GetRoutingKey<T>()
        {
            var type = typeof(T);
            return GetRoutingKey(type);
        }
    }
}
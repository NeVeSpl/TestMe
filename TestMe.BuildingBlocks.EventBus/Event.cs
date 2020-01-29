using System;
using System.Text.Json;

namespace TestMe.BuildingBlocks.EventBus
{
    public class Event
    {
        public Guid EventId { get; private set; }
        public string RoutingKey { get; private set; }
        public string CorrelationId { get; private set; }
        public string Payload { get; private set; }
        public DateTimeOffset? PostDateTime { get; set;}
        public DateTimeOffset? ReceivedDateTime { get; set; }


#pragma warning disable CS8618 // Non-nullable field is uninitialized. Constructor neeed by EF core
        private Event()

        {

        }
#pragma warning restore CS8618

        public Event(object message, string correlationId)
        {
            Type typeOfMessage = message.GetType();
            Payload = JsonSerializer.Serialize(message);
#pragma warning disable CS8601 // Possible null reference assignment.
            RoutingKey = typeOfMessage.FullName;
#pragma warning restore CS8601 // Possible null reference assignment.
            CorrelationId = correlationId;
        }
    }
}
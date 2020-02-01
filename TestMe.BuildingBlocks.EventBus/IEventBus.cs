using System.Threading.Tasks;

namespace TestMe.BuildingBlocks.EventBus
{
    public interface IEventBus
    {
        Task<bool> Publish(Event @event);

        public void Subscribe<TEvent, TEventHandler, TEventInterceptor>(string queueName)
            where TEvent : class
            where TEventHandler : IEventHandler<TEvent>
            where TEventInterceptor : IEventInterceptor;
    }
}

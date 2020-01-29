using System;
using System.Threading.Tasks;

namespace TestMe.BuildingBlocks.EventBus
{
    public interface IEventInterceptor
    {
        Task ReceiveEvent(Event @event, Func<Task> handleEvent);
    }
}

using System.Threading.Tasks;

namespace TestMe.BuildingBlocks.EventBus
{
    public interface IEventHandler<in TEvent>  where TEvent : class
    {
        Task Handle(TEvent @event);
    }
}

using System.Threading.Tasks;

namespace TestMe.BuildingBlocks.EventBus
{
    public interface IEventBus
    {
        Task<bool> Publish(Event @event);

        void Subscribe<T, TH, TEI>()
            where T : class
            where TH : IEventHandler<T>
            where TEI: IEventInterceptor;
    }
}

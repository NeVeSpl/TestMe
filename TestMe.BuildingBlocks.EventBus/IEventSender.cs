using System.Threading;
using System.Threading.Tasks;

namespace TestMe.BuildingBlocks.EventBus
{
    public interface IEventSender
    {
        Task SendEvents(CancellationToken cancellationToken);
    }
}
